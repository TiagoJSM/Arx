using CommonInterfaces.Controllers;
using CommonInterfaces.Enums;
using CommonInterfaces.Weapons;
using MathHelper;
using MathHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Helpers
{
    public delegate void AttackFinished();

    public class AttackHandlerContext
    {
        public float AimAngle { get; set; }
        public int ComboCount { get; set; }
    }
    public interface ICombatComponent : IAnimationController
    {
        GameObject AimingArm { get; }
        GameObject Head { get; }
        float AimingLimit { get; }
        float HeadLookLimit { get; }
        event Action OnChainWeaponThrow;
        void NotifyAttackFinish();
    }
    public interface IAttackHandler : IDisposable
    {
        WeaponType WeaponType { get; }
        IWeapon Weapon { get; }

        void AttackIsOver();
        void FocusPrimaryAttack();
        bool PrimaryAttack(AttackHandlerContext context);
        bool SecundaryAttack();
        void Update();
    }
    public abstract class BaseAttackHandler<TWeapon> : IAttackHandler where TWeapon : IWeapon
    {
        private GameObject _aimingArm;
        private float _aimingLimit;

        public TWeapon Weapon { get; private set; }

        public WeaponType WeaponType
        {
            get
            {
                return Weapon.WeaponType;
            }
        }

        public GameObject AimingArm
        {
            get
            {
                return _aimingArm;
            }
        }

        IWeapon IAttackHandler.Weapon
        {
            get
            {
                return Weapon;
            }
        }

        public BaseAttackHandler(
            TWeapon weapon,
            ICombatComponent combatComponent)
        {
            Weapon = weapon;
            _aimingArm = combatComponent.AimingArm;
            _aimingLimit = combatComponent.AimingLimit;
        }

        public abstract void AttackIsOver();

        public abstract void FocusPrimaryAttack();

        public abstract bool PrimaryAttack(AttackHandlerContext context);

        public abstract bool SecundaryAttack();

        public abstract void Update();

        public abstract void Dispose();

        protected float AimAtTargetRotation(float limit)
        {
            var center = _aimingArm.transform.position;
            var aimPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var inverted = (_aimingArm.transform.lossyScale.x < 0) || (_aimingArm.transform.lossyScale.y < 0);

            var rotation = default(float);
            if (!inverted)
            {
                rotation = FloatUtils.AngleBetween(center, aimPosition);
            }
            else
            {
                rotation = (-FloatUtils.AngleBetween(center, aimPosition) + 180);
            }
            if (rotation > 180)
            {
                rotation -= 360;
            }
            rotation = Mathf.Clamp(rotation, -limit, limit);
            return rotation;
        }

        protected void AimAtTarget(GameObject obj, float limit)
        {
            var rotation = AimAtTargetRotation(limit);
            obj.transform.rotation = Quaternion.Euler(0, 0, rotation);
        }

        protected float GetWeaponAimAngle()
        {
            var center = _aimingArm.transform.position;
            var aimPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var degrees = FloatUtils.AngleBetween(center, aimPosition).ReduceToSingleTurn();
            var radians = degrees * Mathf.Deg2Rad;
            var quadrant = radians.GetQuadrant();

            //ToDo: direction should be provided by ICombatComponent
            var inverted = (_aimingArm.transform.lossyScale.x < 0) || (_aimingArm.transform.lossyScale.y < 0);

            if (!inverted)
            {
                degrees = quadrant == 3 ? degrees - FloatUtils.FullDegreeTurn : degrees;
                degrees = Mathf.Clamp(degrees, -_aimingLimit, _aimingLimit);
            }
            else
            {
                degrees = Mathf.Clamp(degrees, FloatUtils.HalfDegreeTurn - _aimingLimit, FloatUtils.HalfDegreeTurn + _aimingLimit);
            }
            return degrees;
        }
    }
    public class ShooterHandler : BaseAttackHandler<IShooterWeapon>
    {
        private ICombatComponent _combatComponent;

        public ShooterHandler(IShooterWeapon weapon, ICombatComponent combatComponent) : base(weapon, combatComponent)
        {
            _combatComponent = combatComponent;
        }

        public override void AttackIsOver()
        {
        }

        public override void FocusPrimaryAttack()
        {
        }

        public override bool PrimaryAttack(AttackHandlerContext context)
        {
            var degrees = GetWeaponAimAngle();
            return Weapon.Shoot(degrees);
        }

        public override bool SecundaryAttack()
        {
            return false;
        }

        public override void Update()
        {
            AimAtTarget(_combatComponent.Head, _combatComponent.HeadLookLimit);
            AimAtTarget(_combatComponent.AimingArm, _combatComponent.AimingLimit);
            _combatComponent.NotifyAttackFinish();
        }

        public override void Dispose() { }
    }
    public class ChainThrowHandler : BaseAttackHandler<IChainThrowWeapon>
    {
        private ICombatComponent _combatComponent;

        public ChainThrowHandler(IChainThrowWeapon weapon, ICombatComponent combatComponent) : base(weapon, combatComponent)
        {
            _combatComponent = combatComponent;
            _combatComponent.OnChainWeaponThrow += OnChainWeaponThrowHandler;
            Weapon.OnAttackFinish += OnAttackFinishHandler;
        }

        public override void AttackIsOver()
        {
            
        }

        public override void FocusPrimaryAttack()
        {
            Weapon.FocusThrow();
        }

        public override bool PrimaryAttack(AttackHandlerContext context)
        {
            return true;
        }

        public override bool SecundaryAttack()
        {
            Weapon.Spin();
            return true;
        }

        public override void Update() { }

        public override void Dispose()
        {
            _combatComponent.OnChainWeaponThrow -= OnChainWeaponThrowHandler;
        }

        private void OnAttackFinishHandler()
        {
            _combatComponent.NotifyAttackFinish();
        }
        
        private void OnChainWeaponThrowHandler()
        {
            //ToDo: direction should be provided by ICombatComponent
            var inverted = (_combatComponent.AimingArm.transform.lossyScale.x < 0) || (_combatComponent.AimingArm.transform.lossyScale.y < 0);
            Weapon.Throw(inverted ? Direction.Left : Direction.Right);
        }
    }
    public class CloseCombatHandler : BaseAttackHandler<ICloseCombatWeapon>
    {
        private ICombatComponent _combatComponent;

        public CloseCombatHandler(
            ICloseCombatWeapon weapon,
            ICombatComponent combatComponent)
            : base(weapon, combatComponent)
        {
            _combatComponent = combatComponent;
        }

        public override void AttackIsOver()
        {
            Weapon.AttackIsOver();
        }

        public override void FocusPrimaryAttack()
        {
        }

        public override bool PrimaryAttack(AttackHandlerContext context)
        {
            Weapon.StartLightAttack(context.ComboCount);
            return true;
        }

        public override bool SecundaryAttack()
        {
            Weapon.StartStrongAttack();
            return true;
        }

        public override void Update()
        {
            if (_combatComponent.IsCurrentAnimationOver)
            {
                _combatComponent.NotifyAttackFinish();
            }
        }

        public override void Dispose() { }
    }
    public static class AttackHandlerHelper
    {
        public static IAttackHandler GetHandlerFor(
            IWeapon weapon, 
            ICombatComponent combatComponent)
        {
            if (weapon is IShooterWeapon)
            {
                return new ShooterHandler(weapon as IShooterWeapon, combatComponent);
            }
            if (weapon is IChainThrowWeapon)
            {
                return new ChainThrowHandler(weapon as IChainThrowWeapon, combatComponent);
            }
            if (weapon is ICloseCombatWeapon)
            {
                return new CloseCombatHandler(weapon as ICloseCombatWeapon, combatComponent);
            }
            return null;
        }
    }
}
