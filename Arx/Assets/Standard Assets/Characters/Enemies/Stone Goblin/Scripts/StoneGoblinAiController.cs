using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonInterfaces.Enums;
using UnityEngine;
using Assets.Standard_Assets._2D.Scripts.Characters.CommonAiBehaviour;
using Assets.Standard_Assets.Scripts.StateMachine;
using Assets.Standard_Assets.Common;
using Assets.Standard_Assets.Extensions;
using Assets.Standard_Assets._2D.Scripts.Characters;
using Assets.Standard_Assets._2D.Scripts.Controllers;
using System.Collections;
using Assets.Standard_Assets._2D.Scripts.Hazards;

namespace Assets.Standard_Assets.Characters.Enemies.Stone_Goblin.Scripts
{
    public class StoneGoblinAiStateManager : StateManager<StoneGoblinAiController, object>
    {
        public StoneGoblinAiStateManager(StoneGoblinAiController controller) : base(controller)
        {
            this.SetInitialState<IddleState<StoneGoblinAiController>>()
                .To<AttackedState<StoneGoblinAiController>>((c, a, t) => c.Attacked)
                .To<FollowState<StoneGoblinAiController>>((c, a, t) => c.Target != null)
                .To<RollAttackState>((c, a, t) => c.CanRollAttack);

            this.From<FollowState<StoneGoblinAiController>>()
                .To<AttackedState<StoneGoblinAiController>>((c, a, t) => c.Attacked)
                .To<AttackTargetState<StoneGoblinAiController>>((c, a, t) => c.IsTargetInRange)
                .To<IddleState<StoneGoblinAiController>>((c, a, t) => c.Target == null || !c.CanMoveToGroundAhead())
                .To<RollAttackState>((c, a, t) => c.CanRollAttack);

            this.From<AttackTargetState<StoneGoblinAiController>>()
                .To<AttackedState<StoneGoblinAiController>>((c, a, t) => c.Attacked)
                .To<FollowState<StoneGoblinAiController>>((c, a, t) => !c.IsTargetInRange && !c.Attacking)
                .To<AttackTargetState<StoneGoblinAiController>>((c, a, t) => c.IsTargetInRange && !c.Attacking)
                .To<RollAttackState>((c, a, t) => c.CanRollAttack);

            this.From<AttackedState<StoneGoblinAiController>>()
               .To<FollowState<StoneGoblinAiController>>((c, a, t) => !c.Attacked);

            this.From<RollAttackState>()
                .To<FollowState<StoneGoblinAiController>>((c, a, t) => !c.Rolling);

            this.FromAny()
                .To<DeadState<StoneGoblinAiController>>((c, a, t) => c.Dead);
        }
    }

    [RequireComponent(typeof(MeleeEnemyController))]
    [RequireComponent(typeof(StoneGoblinController))]
    [RequireComponent(typeof(DamageOnTouch))]
    public class StoneGoblinAiController : AbstractPlatformerCharacterAiController, ICharacterAI
    {
        private MeleeEnemyController _meleeEnemy;
        private StoneGoblinController _stoneGoblinController;
        private DamageOnTouch _damageOnTouch;
        private StoneGoblinAiStateManager _stateManager;
        private bool _rollInCooldown = false;

        [SerializeField]
        private CharacterFinder _characterFinder;
        [SerializeField]
        private float _playerMaxDistanceForRoll = 10;
        [SerializeField]
        private float _rollCooldown = 6;
        [SerializeField]
        private LayerMask _rollLineOfSightMask;

        public bool Attacking { get { return _meleeEnemy.InPain; } }

        public bool Attacked { get { return _meleeEnemy.InPain; } }
        public bool Dead { get { return _meleeEnemy.Dead; } }
        public bool Rolling { get { return _stoneGoblinController.Rolling; } }

        protected override Direction CurrentDirection { get { return _meleeEnemy.Direction; } }

        protected override Vector2 Velocity { get { return _meleeEnemy.Velocity; } }

        public bool CanRollAttack 
        {
            get
            {
                return !_rollInCooldown && IsPlayerInLineOfSight();
            }
        }

        public override void Move(float directionValue)
        {
            _meleeEnemy.Move(directionValue);
        }

        public void MoveToTarget()
        {
            _meleeEnemy.MovementType = MovementType.Run;
            SetActiveCoroutine(
                CoroutineHelpers.FollowTargetCoroutine(
                    transform,
                    Target,
                    movement => _meleeEnemy.Move(movement),
                    () => IsTargetInRange));
        }

        public void StartIddle()
        {
            _meleeEnemy.MovementType = MovementType.Walk;
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
            _meleeEnemy.OrderAttack();
        }

        public void RollAttack()
        {
            _meleeEnemy.enabled = false;
            _stoneGoblinController.enabled = true;

            _stoneGoblinController.RollAttack(_meleeEnemy.Direction);
        }

        public void RollAttackOver()
        {
            _meleeEnemy.enabled = true;
            _stoneGoblinController.enabled = false;
            StartCoroutine(RollAttackCooldown());
        }

        private IEnumerator RollAttackCooldown()
        {
            _rollInCooldown = true;
            yield return new WaitForSeconds(_rollCooldown);
            _rollInCooldown = false;
        }

        protected override void Awake()
        {
            base.Awake();
            _stateManager = new StoneGoblinAiStateManager(this);
            _meleeEnemy = GetComponent<MeleeEnemyController>();
            _stoneGoblinController = GetComponent<StoneGoblinController>();
            _damageOnTouch = GetComponent<DamageOnTouch>();

            _meleeEnemy.enabled = true;
            _stoneGoblinController.enabled = false;
            _characterFinder.OnCharacterFound += OnCharacterFoundHandler;
        }

        private void OnCharacterFoundHandler(BasePlatformerController controller)
        {
            Target = controller.gameObject;
        }

        private void Update()
        {
            _stateManager.Perform(null);
        }

        private bool IsPlayerInLineOfSight()
        {
            if(Target == null)
            {
                return false;
            }
            var distance = Vector2.Distance(Target.transform.position, transform.position);
            if(distance > _playerMaxDistanceForRoll)
            {
                return false;
            }

            var direction = (Target.transform.position - transform.position).normalized;

            var hit = Physics2D.Raycast(transform.position, direction, _playerMaxDistanceForRoll, _rollLineOfSightMask);
            return hit.transform.gameObject == Target;
        }
    }
}
