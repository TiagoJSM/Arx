using CommonInterfaces.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericComponents.Helpers
{
    public class AttackHandlerContext
    {
        public float AimAngle { get; set; }
        public int ComboCount { get; set; }
    }
    public interface IAttackHandler
    {
        void AttackIsOver();
        void FocusPrimaryAttack();
        bool PrimaryAttack(AttackHandlerContext context);
        bool SecundaryAttack();
    }
    public class ShooterHandler : IAttackHandler
    {
        private IShooterWeapon _weapon;
        public ShooterHandler(IShooterWeapon weapon)
        {
            _weapon = weapon;
        }

        public void AttackIsOver()
        {
        }

        public void FocusPrimaryAttack()
        {
        }

        public bool PrimaryAttack(AttackHandlerContext context)
        {
            return _weapon.Shoot(context.AimAngle);
        }

        public bool SecundaryAttack()
        {
            return false;
        }
    }
    public class ChainThrowHandler : IAttackHandler
    {
        private IChainThrowWeapon _weapon;
        public ChainThrowHandler(IChainThrowWeapon weapon)
        {
            _weapon = weapon;
        }

        public void AttackIsOver()
        {
            
        }

        public void FocusPrimaryAttack()
        {
            _weapon.FocusThrow();
        }

        public bool PrimaryAttack(AttackHandlerContext context)
        {
            _weapon.Throw();
            return true;
        }

        public bool SecundaryAttack()
        {
            _weapon.Spin();
            return true;
        }
    }
    public class CloseCombatHandler : IAttackHandler
    {
        private ICloseCombatWeapon _weapon;
        public CloseCombatHandler(ICloseCombatWeapon weapon)
        {
            _weapon = weapon;
        }

        public void AttackIsOver()
        {
            _weapon.AttackIsOver();
        }

        public void FocusPrimaryAttack()
        {
        }

        public bool PrimaryAttack(AttackHandlerContext context)
        {
            _weapon.StartLightAttack(context.ComboCount);
            return true;
        }

        public bool SecundaryAttack()
        {
            _weapon.StartStrongAttack();
            return true;
        }
    }
    public static class AttackHandlerHelper
    {
        public static IAttackHandler GetHandlerFor(IWeapon weapon)
        {
            if (weapon is IShooterWeapon)
            {
                return new ShooterHandler(weapon as IShooterWeapon);
            }
            if (weapon is IChainThrowWeapon)
            {
                return new ChainThrowHandler(weapon as IChainThrowWeapon);
            }
            if (weapon is ICloseCombatWeapon)
            {
                return new CloseCombatHandler(weapon as ICloseCombatWeapon);
            }
            return null;
        }
    }
}
