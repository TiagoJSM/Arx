using Assets.Standard_Assets._2D.Scripts.Characters;
using Assets.Standard_Assets._2D.Scripts.Controllers;
using Assets.Standard_Assets.Characters.CharacterBehaviour;
using Assets.Standard_Assets.Common;
using Assets.Standard_Assets.Extensions;
using Assets.Standard_Assets.Scripts.StateMachine;
using Assets.Standard_Assets.Weapons;
using CommonInterfaces.Enums;
using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utils;

namespace Assets.Standard_Assets.Characters.Enemies.Desert_Thief.Scripts
{
    public class DesertThiefEnemyAiStateManager : StateManager<DesertThiefEnemyAiControl, object>
    {
        public DesertThiefEnemyAiStateManager(DesertThiefEnemyAiControl controller) : base(controller)
        {
            this.SetInitialState<IddleState<DesertThiefEnemyAiControl>>()
                .To<AttackedState<DesertThiefEnemyAiControl>>((c, a, t) => c.Attacked)
                .To<ThrowDaggerState>((c, a, t) => c.Target != null && c.IsTargetInDaggerThrowReacheablePosition() && !c.IsDaggerInCooldown)
                .To<FollowState<DesertThiefEnemyAiControl>>((c, a, t) => c.Target != null);

            this.From<FollowState<DesertThiefEnemyAiControl>>()
                .To<AttackedState<DesertThiefEnemyAiControl>>((c, a, t) => c.Attacked)
                .To<ThrowDaggerState>((c, a, t) => c.Target != null && c.IsTargetInDaggerThrowReacheablePosition() && !c.IsDaggerInCooldown || c.IsAnotherEnemyBetweenTarget)
                .To<AttackTargetState<DesertThiefEnemyAiControl>>((c, a, t) => c.IsTargetInRange)
                .To<DodgeAttackState>((c, a, t) => c.Dodge)
                .To<IddleState<DesertThiefEnemyAiControl>>((c, a, t) => c.Target == null || !c.CanMoveToGroundAhead())
                .To<FollowState<DesertThiefEnemyAiControl>>((c, a, t) => c.Target != null && !c.Following);

            this.From<AttackTargetState<DesertThiefEnemyAiControl>>()
                .To<AttackedState<DesertThiefEnemyAiControl>>((c, a, t) => c.Attacked)
                .To<DodgeAttackState>((c, a, t) => c.Dodge)
                .To<FollowState<DesertThiefEnemyAiControl>>((c, a, t) => !c.IsTargetInRange && !c.Attacking)
                .To<AttackTargetState<DesertThiefEnemyAiControl>>((c, a, t) => c.IsTargetInRange && !c.Attacking);

            this.From<AttackedState<DesertThiefEnemyAiControl>>()
               .To<FollowState<DesertThiefEnemyAiControl>>((c, a, t) => !c.Attacked);

            this.From<ThrowDaggerState>()
                .To<AttackedState<DesertThiefEnemyAiControl>>((c, a, t) => c.Attacked)
                .To<ThrowDaggerState>((c, a, t) => c.Target != null && c.IsTargetInDaggerThrowReacheablePosition() && !c.IsDaggerInCooldown && !c.ThrowDagger || c.IsAnotherEnemyBetweenTarget)
                .To<FollowState<DesertThiefEnemyAiControl>>((c, a, t) => !c.ThrowDagger);

            this.From<DodgeAttackState>()
                .To<FollowState<DesertThiefEnemyAiControl>>((c, a, t) => t > c.DodgeDuration);

            this.FromAny()
                .To<DeadState<DesertThiefEnemyAiControl>>((c, a, t) => c.Dead);
        }
    }

    [RequireComponent(typeof(MeleeEnemyController))]
    [RequireComponent(typeof(CharacterAwarenessNotifier))]
    public class DesertThiefEnemyAiControl : AbstractPlatformerCharacterAiController, ICharacterAI, ICharacterAware
    {
        private AudioSource _currentDaggerThrowSound;
        private Collider2D[] _colliderBuffer;
        private Coroutine _enemyProximityCoroutine;
        private bool _targetWasntAttacking = true;
        private bool _targetStartedAttack;

        [SerializeField]
        private CharacterFinder _characterFinder;
        [SerializeField]
        private Transform _daggerTargetPosition;
        [SerializeField]
        private float _daggerTargetRadius;
        [SerializeField]
        private Projectile _dagger;
        [SerializeField]
        private Transform _daggerThrowOrigin;
        [SerializeField]
        private float _daggerCooldownTime = 4;
        [SerializeField]
        private AudioSource[] _throwDaggerSounds;
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float _dodgeRate = 0.2f;
        [SerializeField]
        private float _dodgeRange = 7.0f;
        [SerializeField]
        private float _dodgeDuration = 1.0f;

        private CharacterAwarenessNotifier _awarenessNotifier;
        private MeleeEnemyController _controller;
        private DesertThiefEnemyAiStateManager _stateManager;
        private float _daggerElapsedCooldown;

        public bool ThrowDagger { get; set; }
        public bool Attacked { get { return _controller.InPain; } }
        public bool Dead { get { return _controller.Dead; } }
        public bool IsDaggerInCooldown
        {
            get
            {
                return _daggerElapsedCooldown < _daggerCooldownTime;
            }
        }
        public bool IsAnotherEnemyBetweenTarget { get; private set; }
        public bool Dodge { get; private set; }
        public bool Dodging { get; private set; }
        public float DodgeDuration { get { return _dodgeDuration; } }

        public bool Attacking
        {
            get
            {
                return _controller.Attacking;
            }
        }

        public override Direction CurrentDirection
        {
            get
            {
                return _controller.Direction;
            }
        }

        protected override Vector2 Velocity
        {
            get
            {
                return _controller.Velocity;
            }
        }
        public bool Following { get; private set; }

        public DesertThiefEnemyAiControl()
        {
            _colliderBuffer = new Collider2D[10];
        }

        public void ThrowDaggerAtEnemy()
        {
            _daggerElapsedCooldown = 0.0f;
            var projectile = Instantiate(_dagger, _daggerThrowOrigin.position, Quaternion.identity);

            //throw the dagger at the character, but not at his feet
            var daggerDirection = (Target.transform.position + new Vector3(0, _daggerThrowOrigin.position.y - transform.position.y)) - _daggerThrowOrigin.position;

            //make sure the dagger is not thrown backwards when enemy moved behind this character
            var throwDirection = MovementUtils.DirectionOfMovement(daggerDirection.x, _controller.Direction);
            if (throwDirection != _controller.Direction)
            {
                daggerDirection.x *= -1;
            }

            projectile.Direction = daggerDirection.normalized;
            projectile.Attacker = gameObject;
        }

        public void MoveToTarget()
        {
            _controller.MovementType = MovementType.Run;
            FollowTarget();
            _enemyProximityCoroutine = StartCoroutine(EnemyToTargetProximityRoutine());
        }

        public void StartIddle()
        {
            _controller.MovementType = MovementType.Walk;
            IddleMovement();
        }

        public void StopIddle()
        {
            StopActiveCoroutine();
        }

        public void StopMoving()
        {
            StopActiveCoroutine();
            Following = false;
            if (_enemyProximityCoroutine != null)
            {
                IsAnotherEnemyBetweenTarget = false;
                StopCoroutine(_enemyProximityCoroutine);
            }
        }

        public void OrderAttack()
        {
            StopActiveCoroutine();
            _controller.OrderAttack();
        }

        public void StartDaggerThrow()
        {
            ThrowDagger = true;
            _currentDaggerThrowSound = _throwDaggerSounds.Random();
            _currentDaggerThrowSound.Play();
        }

        public void StopDaggerThrow()
        {
            ThrowDagger = false;
            if (_currentDaggerThrowSound != null)
            {
                _currentDaggerThrowSound.Stop();
            }
        }

        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();
            _daggerElapsedCooldown = _daggerCooldownTime;
            _controller = GetComponent<MeleeEnemyController>();
            _awarenessNotifier = GetComponent<CharacterAwarenessNotifier>();
            _stateManager = new DesertThiefEnemyAiStateManager(this);
            _characterFinder.OnCharacterFound += OnCharacterFoundHandler;
            _controller.OnAttacked += OnAttackedHandler;
        }

        private void OnAttackedHandler(BasePlatformerController character, GameObject attacker)
        {
            if (attacker.IsPlayer())
            {
                OnCharacterFoundHandler(attacker.GetComponent<BasePlatformerController>());
            }
        }

        public override void Move(float directionValue)
        {
            _controller.Move(directionValue);
        }

        public bool IsTargetInDaggerThrowReacheablePosition()
        {
            if (Target != null)
            {
                var overlap = Physics2D.OverlapCircle(_daggerTargetPosition.position, _daggerTargetRadius, _characterFinder.CharacterLayer);
                return overlap && Target == overlap.gameObject;
            }
            return false;
        }

        public void Aware(GameObject obj)
        {
            Target = obj;
        }

        public void StartDodge()
        {
            _controller.CanBeAttacked = false;
            Dodging = true;
        }

        public void EndDodge()
        {
            _controller.CanBeAttacked = true;
            Dodging = false;
        }

        void Update()
        {
            Dodge = false;
            if (TargetController != null)
            {
                if (_targetWasntAttacking)
                {
                    if (TargetController.Attacking)
                    {
                        _targetWasntAttacking = false;
                        var currentPosition = this.transform.position;
                        var distance = Vector2.Distance(currentPosition, Target.transform.position);
                        var rollChance = UnityEngine.Random.Range(0.0f, 1.0f) <= _dodgeRate;
                        var rollDistance = distance < _dodgeRange;
                        Dodge = rollChance && rollDistance;
                    }
                }
                else
                {
                    if (!TargetController.Attacking)
                    {
                        _targetWasntAttacking = true;
                    }
                }
            }
            _stateManager.Perform(null);
            if (IsDaggerInCooldown)
            {
                _daggerElapsedCooldown += Time.deltaTime;
            }
        }

        void OnDestroy()
        {
            _characterFinder.OnCharacterFound -= OnCharacterFoundHandler;
            _controller.OnAttacked -= OnAttackedHandler;
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_daggerTargetPosition.position, _daggerTargetRadius);
        }

        private void FollowTarget()
        {
            _controller.MovementType = MovementType.Run;
            Following = true;
            SetActiveCoroutine(
                CoroutineHelpers.FollowTargetCoroutine(
                    transform,
                    Target,
                    movement => _controller.Move(movement),
                    () => IsTargetInRange,
                    () => Following = false));
        }

        private void OnCharacterFoundHandler(BasePlatformerController controller)
        {
            if (Target == null)
            {
                Target = controller.gameObject;
                _awarenessNotifier.Notify(Target);
            }
        }

        private IEnumerator EnemyToTargetProximityRoutine()
        {
            while (true)
            {
                IsAnotherEnemyBetweenTarget = CheckIfAnotherEnemyBetweenTarget();
                yield return new WaitForSeconds(1);
            }
        }

        private bool CheckIfAnotherEnemyBetweenTarget()
        {
            if (Target == null)
            {
                return false;
            }

            var distance = Vector2.Distance(Target.transform.position, transform.position);

            var count = Physics2D.OverlapCircleNonAlloc(Target.transform.position, distance, _colliderBuffer, gameObject.GetLayerMask());
            for(var idx = 0; idx < count; idx++)
            {
                var collider = _colliderBuffer[idx];
                var otherEnemyDistanceToTarget = Vector2.Distance(Target.transform.position, collider.transform.position);
                var distanceToOtherEnemy = Vector2.Distance(collider.gameObject.transform.position, gameObject.transform.position);
                if (collider.gameObject != gameObject && otherEnemyDistanceToTarget < distance && distanceToOtherEnemy < distance)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
