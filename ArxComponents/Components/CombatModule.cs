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
    public class CombatModule : MonoBehaviour
    {
        private const int MAX_COMBOS = 3;
        private const int COMBO_START = 1;

        private AttackType? _comboType;
        private IWeapon _weapon;
        private IAttackHandler _attackHandler;

        public GameObject aimingArm;
        [Range(0, 90)]
        public float aimLimit = 90;

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
                _attackHandler = AttackHandlerHelper.GetHandlerFor(_weapon, AttackFinished, AnimationController, aimingArm, aimLimit);
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

        public void AttackFinished()
        {
            OnAttackFinish?.Invoke();
        }

        void Update()
        {
            if(_attackHandler != null)
            {
                _attackHandler.Update();
            }
        }
    }
}
