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
        private const int MAX_COMBOS = 3;
        private const int COMBO_START = 1;

        private AttackType _comboType;
        private IWeapon _weapon;
        private IAttackHandler _attackHandler;

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
                //return AnimationController.IsCurrentAnimationOver;
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

        public void AttackIsOver()
        {
            _attackHandler.AttackIsOver();
        }

        private void DoAttack(AttackType attackType)
        {
            Debug.Log("doAttack");
            _comboType = attackType;
            if (attackType == AttackType.None)
            {
                ComboNumber = 0;
            }
            else if (attackType == AttackType.Secundary)
            {
                ComboNumber++;
                if (ComboNumber > MAX_COMBOS)
                {
                    ComboNumber = COMBO_START;
                }
                _attackHandler.SecundaryAttack();
            }
            else if (attackType == AttackType.Primary)
            {
                ComboNumber++;
                if (ComboNumber > MAX_COMBOS)
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
            //_comboType = AttackType.None;
            OnAttackFinish?.Invoke();
        }

        public void ThrowChainWeapon()
        {
            OnChainWeaponThrow?.Invoke();
        }

        bool _over = false;
        public void AttackIsOverCB()
        {
            OnAttackFinish?.Invoke();
            _over = true;
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
