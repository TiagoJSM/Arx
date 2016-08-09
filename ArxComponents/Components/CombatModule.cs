using CommonInterfaces.Controllers;
using CommonInterfaces.Weapons;
using GenericComponents.Enums;
using GenericComponents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components
{
    public class CombatModule : MonoBehaviour, ICombatComponent
    {
        private const int COMBO_START = 1;

        private AttackType _comboType;
        private IWeapon _weapon;
        private IAttackHandler _attackHandler;
        private bool _over = false;

        [SerializeField]
        private int maxCombos = 3;

        public GameObject aimingArm;
        public GameObject head;
        [Range(0, 90)]
        public float aimLimit = 90;
        [Range(0, 90)]
        public float headLookLimit = 90;

        public event Action OnAttackFinish;
        public event Action OnChainWeaponThrow;

        public IWeapon Weapon
        {
            get
            {
                return _weapon;
            }
            set
            {
                _weapon = value;
                if(_attackHandler != null)
                {
                    _attackHandler.Dispose();
                }
                _attackHandler = AttackHandlerHelper.GetHandlerFor(_weapon, this);
            }
        }

        public int ComboNumber { get; set; }

        public WeaponType? WeaponType
        {
            get
            {
                if(_attackHandler == null)
                {
                    return null;
                }
                return _attackHandler.WeaponType;
            }
        }

        public IAnimationController AnimationController { get; set; }

        public GameObject AimingArm
        {
            get
            {
                return aimingArm;
            }
        }

        public float AimingLimit
        {
            get
            {
                return aimLimit;
            }
        }

        public AttackType ComboType
        {
            get
            {
                return _comboType;
            }
        }

        public bool IsCurrentAnimationOver
        {
            get
            {
                return _over;
            }
        }

        public GameObject Head
        {
            get
            {
                return head;
            }
        }

        public float HeadLookLimit
        {
            get
            {
                return headLookLimit;
            }
        }

        public void PrimaryAttack()
        {
            DoAttack(AttackType.Primary);
        }

        public void SecundaryAttack()
        {
            DoAttack(AttackType.Secundary);
        }

        private void DoAttack(AttackType attackType)
        {
            _comboType = attackType;
            if (attackType == AttackType.None)
            {
                ComboNumber = 0;
            }
            else if (attackType == AttackType.Secundary)
            {
                ComboNumber++;
                if (ComboNumber > maxCombos)
                {
                    ComboNumber = COMBO_START;
                }
                _attackHandler.SecundaryAttack();
            }
            else if (attackType == AttackType.Primary)
            {
                ComboNumber++;
                if (ComboNumber > maxCombos)
                {
                    ComboNumber = COMBO_START;
                }
                _attackHandler.PrimaryAttack(
                    new AttackHandlerContext()
                    {
                        ComboCount = ComboNumber
                    });
            }
        }

        public void NotifyAttackFinish()
        {
            _attackHandler.AttackIsOver();
            OnAttackFinish?.Invoke();
            _over = true;
        }

        public void ThrowChainWeapon()
        {
            OnChainWeaponThrow?.Invoke();
        }

        void Update()
        {
            if(_attackHandler != null)
            {
                _attackHandler.Update();
            }
            _over = false;
        }
    }
}
