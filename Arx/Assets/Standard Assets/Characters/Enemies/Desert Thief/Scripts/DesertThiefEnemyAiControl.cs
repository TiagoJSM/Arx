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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Enemies.Desert_Thief.Scripts
{
    public class DesertThiefEnemyAiStateManager : StateManager<DesertThiefEnemyAiControl, object>
    {
        public DesertThiefEnemyAiStateManager(DesertThiefEnemyAiControl controller) : base(controller)
        {
            this.SetInitialState<IddleState<DesertThiefEnemyAiControl>>()
                .To<AttackedState<DesertThiefEnemyAiControl>>((c, a, t) => c.Attacked)
                .To<ThrowDaggerState>((c, a, t) => c.Target != null && c.IsTargetInDaggerThrowReacheablePosiion() && !c.IsDaggerInCooldown)
                .To<FollowState<DesertThiefEnemyAiControl>>((c, a, t) => c.Target != null);

            this.From<FollowState<DesertThiefEnemyAiControl>>()
                .To<AttackedState<DesertThiefEnemyAiControl>>((c, a, t) => c.Attacked)
                .To<ThrowDaggerState>((c, a, t) => c.Target != null && c.IsTargetInDaggerThrowReacheablePosiion() && !c.IsDaggerInCooldown)
                .To<AttackTargetState<DesertThiefEnemyAiControl>>((c, a, t) => c.IsTargetInRange)
                .To<IddleState<DesertThiefEnemyAiControl>>((c, a, t) => c.Target == null || !c.CanMoveToGroundAhead());

            this.From<AttackTargetState<DesertThiefEnemyAiControl>>()
                .To<AttackedState<DesertThiefEnemyAiControl>>((c, a, t) => c.Attacked)
                .To<FollowState<DesertThiefEnemyAiControl>>((c, a, t) => !c.IsTargetInRange && !c.Attacking)
                .To<AttackTargetState<DesertThiefEnemyAiControl>>((c, a, t) => c.IsTargetInRange && !c.Attacking);

            this.From<AttackedState<DesertThiefEnemyAiControl>>()
               .To<FollowState<DesertThiefEnemyAiControl>>((c, a, t) => !c.Attacked);

            this.From<ThrowDaggerState>()
                .To<AttackedState<DesertThiefEnemyAiControl>>((c, a, t) => c.Attacked)
                .To<ThrowDaggerState>((c, a, t) => c.Target != null && c.IsTargetInDaggerThrowReacheablePosiion() && !c.IsDaggerInCooldown)
                .To<FollowState<DesertThiefEnemyAiControl>>((c, a, t) => !c.ThrowDagger);

            this.FromAny()
                .To<DeadState<DesertThiefEnemyAiControl>>((c, a, t) => c.Dead);
        }
    }

    [RequireComponent(typeof(MeleeEnemyController))]
    [RequireComponent(typeof(CharacterAwarenessNotifier))]
    public class DesertThiefEnemyAiControl : AbstractPlatformerCharacterAiController, ICharacterAI, ICharacterAware
    {
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

        public bool Attacking
        {
            get
            {
                return _controller.Attacking;
            }
        }

        protected override Direction CurrentDirection
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

        public void ThrowDaggerAtEnemy()
        {
            _daggerElapsedCooldown = 0.0f;
            var projectile = Instantiate(_dagger, _daggerThrowOrigin.position, Quaternion.identity);
            projectile.Direction = new Vector3(CurrentDirection.DirectionValue(), 0);
            projectile.Attacker = gameObject;
        }

        public void MoveToTarget()
        {
            _controller.MovementType = MovementType.Run;
            FollowTarget();
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
        }

        public void OrderAttack()
        {
            StopActiveCoroutine();
            _controller.OrderAttack();
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

        public bool IsTargetInDaggerThrowReacheablePosiion()
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

        void Update()
        {
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
            SetActiveCoroutine(
                CoroutineHelpers.FollowTargetCoroutine(
                    transform,
                    Target,
                    movement => _controller.Move(movement),
                    () => IsTargetInRange));
        }

        private void OnCharacterFoundHandler(BasePlatformerController controller)
        {
            if (Target == null)
            {
                Target = controller.gameObject;
                _awarenessNotifier.Notify(Target);
            }
        }
    }
}
