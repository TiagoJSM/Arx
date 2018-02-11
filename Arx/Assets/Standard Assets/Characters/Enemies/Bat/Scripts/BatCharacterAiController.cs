using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonInterfaces.Enums;
using Assets.Standard_Assets._2D.Scripts.Characters.CommonAiBehaviour;
using UnityEngine;
using Assets.Standard_Assets._2D.Scripts.Characters;
using GenericComponents.Controllers.Characters;
using Assets.Standard_Assets._2D.Scripts.Controllers;
using Assets.Standard_Assets.Scripts.StateMachine;

namespace Assets.Standard_Assets.Characters.Enemies.Bat.Scripts
{
    public class BatCharacterAiStateManager : StateManager<BatCharacterAiController, object>
    {
        public BatCharacterAiStateManager(BatCharacterAiController controller) : base(controller)
        {
            this.SetInitialState<IddleState<BatCharacterAiController>>()
                .To<AttackTargetState<BatCharacterAiController>>((c, o, t) => c.Target != null);

            this.From<AttackTargetState<BatCharacterAiController>>()
                .To<MoveAwayFromTargetState>((c, a, t) => !c.Attacking);

            this.From<MoveAwayFromTargetState>()
                .To<IddleState<BatCharacterAiController>>((c, a, t) => c.Target == null)
                .To<AttackTargetState<BatCharacterAiController>>((c, a, t) => c.MovedAwayFromTarget);
        }
    }

    [RequireComponent(typeof(BatCharacterController))]
    public class BatCharacterAiController : FlyingCharacterAiController, ICharacterAI
    {
        private BatCharacterController _controller;
        private BatCharacterAiStateManager _stateManager;

        [SerializeField]
        private CharacterFinder _characterFinder;
        [SerializeField]
        private float _targetRadius = 10;

        public GameObject Target { get; private set; }

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
                return true;
            }
        }

        public bool MovedAwayFromTarget { get; private set; }

        protected override void Move(Vector2 direction)
        {
            _controller.MovementDirection = direction;
        }

        protected override void Awake()
        {
            base.Awake();
            _controller = GetComponent<BatCharacterController>();
            _characterFinder.OnCharacterFound += OnCharacterFoundHandler;
            _stateManager = new BatCharacterAiStateManager(this);
        }

        public void MoveToTarget()
        {
            throw new NotImplementedException();
        }

        public void StopMoving()
        {
            _controller.MovementDirection = Vector2.zero;
        }

        public void StartIddle()
        {
            IddleMovement();
        }

        public void StopIddle()
        {
            StopActiveCoroutine();
        }

        public void OrderAttack()
        {
            _controller.Attack(Target);
        }

        public void MoveAwayFromTarget()
        {
            MovedAwayFromTarget = false;
            SetActiveCoroutine(
                MoveTo(
                    () => StartingPosition, 
                    () => MovedAwayFromTarget = true));
        }

        public void Move(float direction)
        {
            throw new NotImplementedException();
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(StartingPosition, _targetRadius);
        }

        private void Update()
        {
            CheckTargetInRange();
            _stateManager.Perform(null);
        }

        private void OnDestroy()
        {
            _characterFinder.OnCharacterFound -= OnCharacterFoundHandler;
        }

        private void OnCharacterFoundHandler(BasePlatformerController controller)
        {
            if (Target == null)
            {
                Target = controller.gameObject;
            }
        }

        private void CheckTargetInRange()
        {
            if (Target != null)
            {
                var distance = Vector2.Distance(StartingPosition, Target.transform.position);
                if (distance > _targetRadius)
                {
                    Target = null;
                }
            }
        }

        public void OnAttacked()
        {
            throw new NotImplementedException();
        }
    }
}
