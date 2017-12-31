using Assets.Standard_Assets._2D.Scripts.Characters;
using Assets.Standard_Assets._2D.Scripts.Controllers;
using Assets.Standard_Assets.Common;
using Assets.Standard_Assets.Extensions;
using CommonInterfaces.Enums;
using Extensions;
using GenericComponents.StateMachine;
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
                .To<ThrowDaggerState>((c, a, t) => c.Target != null && c.IsTargetInDaggerThrowReacheablePosiion())
                .To<FollowState<DesertThiefEnemyAiControl>>((c, a, t) => c.Target != null);

            this.From<FollowState<DesertThiefEnemyAiControl>>()
                .To<AttackedState<DesertThiefEnemyAiControl>>((c, a, t) => c.Attacked)
                .To<ThrowDaggerState>((c, a, t) => c.Target != null && c.IsTargetInDaggerThrowReacheablePosiion())
                .To<AttackTargetState<DesertThiefEnemyAiControl>>((c, a, t) => c.IsTargetInRange)
                .To<IddleState<DesertThiefEnemyAiControl>>((c, a, t) => c.Target == null || !c.CanMoveToGroundAhead());

            this.From<AttackTargetState<DesertThiefEnemyAiControl>>()
                .To<AttackedState<DesertThiefEnemyAiControl>>((c, a, t) => c.Attacked)
                .To<FollowState<DesertThiefEnemyAiControl>>((c, a, t) => !c.IsTargetInRange && !c.Attacking)
                .To<AttackTargetState<DesertThiefEnemyAiControl>>((c, a, t) => c.IsTargetInRange && !c.Attacking);

            this.From<AttackedState<DesertThiefEnemyAiControl>>()
               .To<FollowState<DesertThiefEnemyAiControl>>((c, a, t) => !c.Attacked);

            this.FromAny()
                .To<DeadState<DesertThiefEnemyAiControl>>((c, a, t) => c.Dead);
        }
    }

    [RequireComponent(typeof(MeleeEnemyController))]
    public class DesertThiefEnemyAiControl : PlatformerCharacterAiControl, ICharacterAI
    {
        [SerializeField]
        private CharacterFinder _characterFinder;
        [SerializeField]
        private float _attackRange = 1;
        [SerializeField]
        private Transform _daggerThrowPosition;
        [SerializeField]
        private float _daggerThrowRadius;

        private GameObject _target;
        private MeleeEnemyController _controller;
        private DesertThiefEnemyAiStateManager _stateManager;

        public bool ThrowDagger { get; set; }
        public bool Attacked { get { return _controller.InPain; } }
        public bool Dead { get { return _controller.Dead; } }

        public GameObject Target
        {
            get
            {
                return _target;
            }
        }

        public bool Attacking
        {
            get
            {
                return _controller.Attacking;
            }
        }

        public bool IsTargetInRange
        {
            get
            {
                var currentPosition = this.transform.position;
                var distance = Vector2.Distance(currentPosition, _target.transform.position);
                return distance < _attackRange;
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
            ThrowDagger = true;
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
            _controller = GetComponent<MeleeEnemyController>();
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
                var overlap = Physics2D.OverlapCircle(_daggerThrowPosition.position, _daggerThrowRadius, _characterFinder.CharacterLayer);
                return overlap && Target == overlap.gameObject;
            }
            return false;
        }

        void Update()
        {
            _stateManager.Perform(null);
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
            Gizmos.DrawWireSphere(_daggerThrowPosition.position, _daggerThrowRadius);
        }

        private void FollowTarget()
        {
            _controller.MovementType = MovementType.Run;
            SetActiveCoroutine(
                CoroutineHelpers.FollowTargetCoroutine(
                    transform,
                    _target,
                    movement => _controller.Move(movement),
                    () => IsTargetInRange));
        }

        private void OnCharacterFoundHandler(BasePlatformerController controller)
        {
            if (_target == null)
            {
                _target = controller.gameObject;
            }
        }
    }
}
