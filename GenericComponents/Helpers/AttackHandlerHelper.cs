using CommonInterfaces.Controllers;
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
    public interface IAttackHandler
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
        public TWeapon Weapon { get; private set; }

        public WeaponType WeaponType
        {
            get
            {
                return Weapon.WeaponType;
            }
        }

        IWeapon IAttackHandler.Weapon
        {
            get
            {
                return Weapon;
            }
        }

        public BaseAttackHandler(TWeapon weapon)
        {
            Weapon = weapon;
        }

        public abstract void AttackIsOver();

        public abstract void FocusPrimaryAttack();

        public abstract bool PrimaryAttack(AttackHandlerContext context);

        public abstract bool SecundaryAttack();

        public abstract void Update();
    }
    public class ShooterHandler : BaseAttackHandler<IShooterWeapon>
    {
        private AttackFinished _attackFinished;
        private GameObject _aimingArm;
        private float _aimingLimit;

        public ShooterHandler(IShooterWeapon weapon, AttackFinished attackFinished, GameObject aimingArm, float aimingLimit) : base(weapon)
        {
            _attackFinished = attackFinished;
            //Weapon.OnCooldownFinish += OnAttackFinishHandler;
            _aimingArm = aimingArm;
            _aimingLimit = aimingLimit;
        }

        public override void AttackIsOver()
        {
        }

        public override void FocusPrimaryAttack()
        {
        }

        public override bool PrimaryAttack(AttackHandlerContext context)
        {
            var center = _aimingArm.transform.position;
            var aimPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //ToDo: needs angle clamp
            var degrees = FloatUtils.AngleBetween(center, aimPosition).ReduceToSingleTurn();
            var radians = degrees * Mathf.Deg2Rad;
            var quadrant = radians.GetQuadrant();
            switch (quadrant)
            {
                case 1:
                    degrees = Mathf.Clamp(degrees, 0, _aimingLimit);
                    break;
                case 2:
                    degrees = Mathf.Clamp(degrees, FloatUtils.HalfDegreeTurn - _aimingLimit, FloatUtils.HalfDegreeTurn);
                    break;
                case 3:
                    degrees = Mathf.Clamp(degrees, FloatUtils.HalfDegreeTurn, FloatUtils.HalfDegreeTurn + _aimingLimit);
                    break;
                case 4:
                    degrees = Mathf.Clamp(degrees, FloatUtils.FullDegreeTurn - _aimingLimit, FloatUtils.FullDegreeTurn);
                    break;
            }
            Debug.Log(degrees);
            return Weapon.Shoot(degrees);
        }

        public override bool SecundaryAttack()
        {
            return false;
        }

        public override void Update()
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
            if(rotation > 180)
            {
                rotation -= 360;
            }
            rotation = Mathf.Clamp(rotation, -_aimingLimit, _aimingLimit);
            _aimingArm.transform.rotation = Quaternion.Euler(0, 0, rotation);
            _attackFinished();
        }

        private void OnAttackFinishHandler()
        {
            _attackFinished();
        }
    }
    public class ChainThrowHandler : BaseAttackHandler<IChainThrowWeapon>
    {
        private AttackFinished _attackFinished;

        public ChainThrowHandler(IChainThrowWeapon weapon, AttackFinished attackFinished) : base(weapon)
        {
            _attackFinished = attackFinished;
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
            Weapon.Throw();
            return true;
        }

        public override bool SecundaryAttack()
        {
            Weapon.Spin();
            return true;
        }

        public override void Update()
        {
            
        }

        private void OnAttackFinishHandler()
        {
            _attackFinished();
        }
    }
    public class CloseCombatHandler : BaseAttackHandler<ICloseCombatWeapon>
    {
        private AttackFinished _attackFinished;
        private IAnimationController _animationController;

        public CloseCombatHandler(
            ICloseCombatWeapon weapon, 
            AttackFinished attackFinished,
            IAnimationController animationController)
            : base(weapon)
        {
            _attackFinished = attackFinished;
            _animationController = animationController;
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
            if (_animationController.IsCurrentAnimationOver)
            {
                _attackFinished();
            }
        }
    }
    public static class AttackHandlerHelper
    {
        public static IAttackHandler GetHandlerFor(
            IWeapon weapon, 
            AttackFinished attackFinished,
            IAnimationController animationController,
            GameObject aimingArm, 
            float aimingLimit)
        {
            if (weapon is IShooterWeapon)
            {
                return new ShooterHandler(weapon as IShooterWeapon, attackFinished, aimingArm, aimingLimit);
            }
            if (weapon is IChainThrowWeapon)
            {
                return new ChainThrowHandler(weapon as IChainThrowWeapon, attackFinished);
            }
            if (weapon is ICloseCombatWeapon)
            {
                return new CloseCombatHandler(weapon as ICloseCombatWeapon, attackFinished, animationController);
            }
            return null;
        }
    }
}
