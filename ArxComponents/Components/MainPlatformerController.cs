using ArxGame.Components.AnimationControllers;
using ArxGame.Components.Combat;
using CommonInterfaces.Controllers;
using CommonInterfaces.Weapons;
using Extensions;
using GenericComponents.Controllers.Characters;
using GenericComponents.Enums;
using GenericComponents.Interfaces.States.PlatformerCharacter;
using GenericComponents.StateMachine;
using GenericComponents.StateMachine.States.PlatformerCharacter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components
{
    //[RequireComponent(typeof(PlatformerCharacterAnimationController))]
    [RequireComponent(typeof(CombatModule))]
    public class MainPlatformerController : PlatformerCharacterController, IPlatformerCharacterController
    {
        private PhysicsMaterial2D _commonFootMaterial;
        private CombatModule _combatModule;
        //private PlatformerCharacterAnimationController _animationController;
        private StateManager<IPlatformerCharacterController, PlatformerCharacterAction> _stateManager;

        [SerializeField]
        private Collider2D _footCollider;
        [SerializeField]
        private PhysicsMaterial2D _iddleFootMaterial;
        [SerializeField]
        private float _rollingDuration = 1;

        private float _move;
        private float _vertical;
        private bool _jump;

        private AttackType _attackAction;

        public StateManager<IPlatformerCharacterController, PlatformerCharacterAction> StateManager
        {
            get
            {
                return _stateManager;
            }
        }

        public IWeapon Weapon
        {
            get
            {
                return _combatModule.Weapon;
            }
            set
            {
                _combatModule.Weapon = value;
            }
        }

        public bool IsCurrentAnimationOver { get; private set; }

        public bool Attacking { get; private set; }

        public int ComboNumber
        {
            get
            {
                return _combatModule.ComboNumber;
            }
        }

        public WeaponType? WeaponType
        {
            get
            {
                return _combatModule.WeaponType;
            }
        }

        public float RollingDuration
        {
            get
            {
                return _rollingDuration;
            }
        }

        public void Move(float move, float vertical, bool jump)
        {
            _move = move;
            _vertical = vertical;
            _jump = jump;
        }

        public void LightAttack()
        {
            _attackAction = AttackType.Primary;
        }

        public void StrongAttack()
        {
            _attackAction = AttackType.Secundary;
        }

        public void DoPrimaryAttack()
        {
            IsCurrentAnimationOver = false;
            Attacking = true;
            _combatModule.PrimaryAttack();
            _attackAction = AttackType.None;
        }

        public void DoSecundaryAttack()
        {
            _combatModule.SecundaryAttack();
            _attackAction = AttackType.None;
        }

        public override void Duck()
        {
            _combatModule.ComboNumber = 0;
            Attacking = false;
            base.Duck();
        }

        public override void Stand()
        {
            _combatModule.ComboNumber = 0;
            Attacking = false;
            base.Stand();
        }

        public void StartIddle()
        {
            Body.drag = float.MaxValue;
        }

        public void StopIddle()
        {
            Body.drag = 0;
        }

        protected override void Awake()
        {
            base.Awake();
            _combatModule = GetComponent<CombatModule>();
            _stateManager = new PlatformerCharacterStateManager(this, _rollingDuration);
            _combatModule.OnAttackFinish += OnAttackFinishHandler;
            //_combatModule
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            var action = new PlatformerCharacterAction(_move, _vertical, _jump, _attackAction);
            _stateManager.Perform(action);
            _move = 0;
            _vertical = 0;
            _jump = false;
            //LockOnFloor();
        }

        private void OnAttackFinishHandler()
        {
            IsCurrentAnimationOver = true;
        }

        private void LockOnFloor()
        {
            var hit = Physics2D.Raycast(this.transform.position, new Vector2(0, -1), 1, whatIsGround);
            Debug.Log(hit.normal);
            var currentPosition = this.transform.position;
            if (hit.collider != null && (currentPosition.y - hit.point.y) > 0.5f)
            {                
                this.transform.position = new Vector3(currentPosition.x, hit.point.y, currentPosition.z);
                Body.position = this.transform.position.ToVector2();
            }
        }
    }
}
