using UnityEngine;
using System.Collections;
using System;
using GenericComponents.StateMachine;
using Extensions;
using Assets.Standard_Assets._2D.Scripts.Characters;
using CommonInterfaces.Enums;
using GenericComponents.Controllers.Characters;

namespace Assets.Standard_Assets.Characters.Enemies.Canyon_Engineer.Scripts
{
    public class EngineerEnemyAiStateManager : StateManager<EngineerEnemyAiControl, object>
    {
        public EngineerEnemyAiStateManager(EngineerEnemyAiControl controller) : base(controller)
        {
            this.SetInitialState<IddleState<EngineerEnemyAiControl>>()
                .To<SurprisedState>((c, a, t) => c.Target != null && c.CanBeSurprised)
                .To<FollowState<EngineerEnemyAiControl>>((c, a, t) => c.Target != null);

            this.From<FollowState<EngineerEnemyAiControl>>()
                .To<AttackTargetState<EngineerEnemyAiControl>>((c, a, t) => c.IsTargetInRange)
                .To<IddleState<EngineerEnemyAiControl>>((c, a, t) => c.Target == null);

            this.From<AttackTargetState<EngineerEnemyAiControl>>()
                .To<FollowState<EngineerEnemyAiControl>>((c, a, t) => !c.IsTargetInRange && !c.Attacking);

            this.From<SurprisedState>()
                .To<FollowState<EngineerEnemyAiControl>>((c, a, t) => !c.IsSurprise);
        }
    }

    [RequireComponent(typeof(MeleeEnemyController))]
    public class EngineerEnemyAiControl : PlatformerCharacterAiControl, ICharacterAI
    {
        [SerializeField]
        private CharacterFinder _characterFinder;
        [SerializeField]
        private float _attackRange = 1;
        [SerializeField]
        private bool _canBeSurprised;
        [SerializeField]
        private float _surprisedTime = 1f;

        private GameObject _target;
        private MeleeEnemyController _controller;
        private EngineerEnemyAiStateManager _stateManager;

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
            _stateManager = new EngineerEnemyAiStateManager(this);
            _characterFinder.OnCharacterFound += OnCharacterFoundHandler;
        }

        protected override void Move(float directionValue)
        {
            _controller.Move(directionValue);
        }

        void Update()
        {
            _stateManager.Perform(null);
        }

        void OnDestroy()
        {
            _characterFinder.OnCharacterFound -= OnCharacterFoundHandler;
        }

        private void FollowTarget()
        {
            _controller.MovementType = MovementType.Run;
            SetActiveCoroutine(FollowTargetCoroutine());
        }

        private void OnCharacterFoundHandler(BasePlatformerController controller)
        {
            if (_target == null)
            {
                _target = controller.gameObject;
            }
        }

        private IEnumerator FollowTargetCoroutine()
        {
            if (_target == null)
            {
                yield break;
            }

            while (true)
            {
                if (IsTargetInRange)
                {
                    yield break;
                }
                var currentPosition = this.transform.position;
                var xDifference = _target.transform.position.x - currentPosition.x;
                _controller.Move(xDifference);
                yield return null;
            }
        }
    }
}
