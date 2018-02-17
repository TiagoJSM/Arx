using UnityEngine;
using System.Collections;
using System;
using Extensions;
using Assets.Standard_Assets._2D.Scripts.Characters;
using CommonInterfaces.Enums;
using GenericComponents.Controllers.Characters;
using Assets.Standard_Assets.Extensions;
using Assets.Standard_Assets.Common;
using Assets.Standard_Assets._2D.Scripts.Controllers;
using Assets.Standard_Assets.Characters.CharacterBehaviour;
using Assets.Standard_Assets.Scripts.StateMachine;

namespace Assets.Standard_Assets.Characters.Enemies.Canyon_Engineer.Scripts
{
    public class EngineerEnemyAiStateManager : StateManager<EngineerEnemyAiControl, object>
    {
        public EngineerEnemyAiStateManager(EngineerEnemyAiControl controller) : base(controller)
        {
            this.SetInitialState<IddleState<EngineerEnemyAiControl>>()
                .To<AttackedState<EngineerEnemyAiControl>>((c, a, t) => c.Attacked)
                .To<SurprisedState>((c, a, t) => c.Target != null && c.CanBeSurprised)
                .To<FollowState<EngineerEnemyAiControl>>((c, a, t) => c.Target != null);

            this.From<FollowState<EngineerEnemyAiControl>>()
                .To<AttackedState<EngineerEnemyAiControl>>((c, a, t) => c.Attacked)
                .To<AttackTargetState<EngineerEnemyAiControl>>((c, a, t) => c.IsTargetInRange)
                .To<IddleState<EngineerEnemyAiControl>>((c, a, t) => c.Target == null);

            this.From<AttackTargetState<EngineerEnemyAiControl>>()
                .To<AttackedState<EngineerEnemyAiControl>>((c, a, t) => c.Attacked)
                .To<FollowState<EngineerEnemyAiControl>>((c, a, t) => !c.IsTargetInRange && !c.Attacking)
                .To<AttackTargetState<EngineerEnemyAiControl>>((c, a, t) => c.IsTargetInRange && !c.Attacking);

            this.From<SurprisedState>()
                .To<AttackedState<EngineerEnemyAiControl>>((c, a, t) => c.Attacked)
                .To<FollowState<EngineerEnemyAiControl>>((c, a, t) => !c.IsSurprise);

            this.From<AttackedState<EngineerEnemyAiControl>>()
               .To<FollowState<EngineerEnemyAiControl>>((c, a, t) => !c.Attacked);

            this.FromAny()
                .To<DeadState<EngineerEnemyAiControl>>((c, a, t) => c.Dead);
        }
    }

    [RequireComponent(typeof(MeleeEnemyController))]
    [RequireComponent(typeof(CharacterAwarenessNotifier))]
    public class EngineerEnemyAiControl : AbstractPlatformerCharacterAiController, ICharacterAI, ICharacterAware
    {
        [SerializeField]
        private CharacterFinder _characterFinder;
        [SerializeField]
        private bool _canBeSurprised;
        [SerializeField]
        private float _surprisedTime = 1f;

        private CharacterAwarenessNotifier _awarenessNotifier;
        private MeleeEnemyController _controller;
        private EngineerEnemyAiStateManager _stateManager;

        public bool Attacked { get { return _controller.InPain; } }
        public bool Dead { get { return _controller.Dead; } }

        public bool Attacking
        {
            get
            {
                return _controller.Attacking;
            }
        }

        public bool CanBeSurprised
        {
            get
            {
                return _canBeSurprised;
            }
        }

        public bool IsSurprise { get; private set; }

        public event Action OnSurprised;

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

        public void ShowSurprise()
        {
            SetActiveCoroutine(ShowSurpriseRoutine());
        }

        private IEnumerator ShowSurpriseRoutine()
        {
            if(OnSurprised != null)
            {
                OnSurprised();
            }
            IsSurprise = true;
            yield return new WaitForSeconds(_surprisedTime);
            IsSurprise = false;
        }

        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();
            _controller = GetComponent<MeleeEnemyController>();
            _awarenessNotifier = GetComponent<CharacterAwarenessNotifier>();
            _stateManager = new EngineerEnemyAiStateManager(this);
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

        public void Aware(GameObject obj)
        {
            Target = obj;
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
