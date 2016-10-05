using CommonInterfaces.Controllers;
using CommonInterfaces.Weapons;
using GenericComponents.Enums;
using GenericComponents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components.Combat
{
    [RequireComponent(typeof(CloseCombatBehaviour))]
    [RequireComponent(typeof(ChainThrowCombatBehaviour))]
    [RequireComponent(typeof(ShooterCombatBehaviour))]
    public class CombatModule : MonoBehaviour, ICombatComponent
    {
        private BaseCombatBehaviour _activeCombatBehaviour;

        private CloseCombatBehaviour _closeCombat;
        private ChainThrowCombatBehaviour _chainThrowCombat;
        private ShooterCombatBehaviour _shooterCombat;

        private IWeapon _weapon;
        private bool _over = false;

        public event Action OnAttackFinish;

        public IWeapon Weapon
        {
            get
            {
                return _weapon;
            }
            set
            {
                _weapon = value;

                if(_activeCombatBehaviour != null)
                {
                    _activeCombatBehaviour.enabled = false;
                }

                if (_weapon is IShooterWeapon)
                {
                    _activeCombatBehaviour = _shooterCombat;
                    _shooterCombat.Weapon = _weapon as IShooterWeapon;
                    _shooterCombat.enabled = true;
                }
                else if (_weapon is IChainThrowWeapon)
                {
                    _activeCombatBehaviour = _chainThrowCombat;
                    _chainThrowCombat.Weapon = _weapon as IChainThrowWeapon;
                    _chainThrowCombat.enabled = true;
                }
                else if (_weapon is ICloseCombatWeapon)
                {
                    _activeCombatBehaviour = _closeCombat;
                    _closeCombat.Weapon = _weapon as ICloseCombatWeapon;
                    _closeCombat.enabled = true;
                }
            }
        }

        public int ComboNumber
        {
            get
            {
                return _activeCombatBehaviour != null ? _activeCombatBehaviour.ComboNumber : 0;
            }
            set
            {
                if(_activeCombatBehaviour != null)
                {
                    _activeCombatBehaviour.ComboNumber = value;
                }
            }
        }

        public WeaponType? WeaponType
        {
            get
            {
                if(_weapon == null)
                {
                    return null;
                }
                return _weapon.WeaponType;
            }
        }

        public AttackType ComboType
        {
            get
            {
                return _activeCombatBehaviour != null ? _activeCombatBehaviour.ComboType : AttackType.None;
            }
        }

        public bool IsCurrentAnimationOver
        {
            get
            {
                return _over;
            }
        }

        public bool PrimaryAttack()
        {
            if(_activeCombatBehaviour != null)
            {
                return _activeCombatBehaviour.PrimaryAttack();
            }
            return false;
        }

        public bool SecundaryAttack()
        {
            if (_activeCombatBehaviour != null)
            {
                return _activeCombatBehaviour.SecundaryAttack();
            }
            return false;
        }
        public void NotifyAttackFinish()
        {
            OnAttackFinish?.Invoke();
            _over = true;
        }

        void Awake()
        {
            _closeCombat = GetComponent<CloseCombatBehaviour>();
            _shooterCombat = GetComponent<ShooterCombatBehaviour>();
            _chainThrowCombat = GetComponent<ChainThrowCombatBehaviour>();

            _closeCombat.OnAttackFinish += NotifyAttackFinish;
            _shooterCombat.OnAttackFinish += NotifyAttackFinish;
            _chainThrowCombat.OnAttackFinish += NotifyAttackFinish;
        }

        void Update()
        {
            _over = false;
        }
    }
}
