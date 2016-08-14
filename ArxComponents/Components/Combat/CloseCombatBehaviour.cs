using CommonInterfaces.Controllers;
using CommonInterfaces.Weapons;
using GenericComponents.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components.Combat
{
    public class CloseCombatBehaviour : BaseGenericCombatBehaviour<ICloseCombatWeapon>
    {
        private const int COMBO_START = 1;

        [SerializeField]
        private int maxCombos = 3;
        [SerializeField]
        private Transform _attackAreaP1;
        [SerializeField]
        private Transform _attackAreaP2;
        [SerializeField]
        private LayerMask _enemyLayer;

        public override event Action OnAttackFinish;

        void Awake()
        {
            this.enabled = false;
        }

        void OnEnable()
        {
            ComboType = AttackType.None;
            ComboNumber = 0;
        }

        public void DoDamage()
        {
            var enemiesInRange = 
                Physics2D
                    .OverlapAreaAll(_attackAreaP1.position, _attackAreaP2.position, _enemyLayer)
                    .Select(c => c.GetComponent<ICharacter>())
                    .Where(c => c != null)
                    .Distinct()
                    .ToArray();

            if (ComboType == AttackType.Primary)
            {
                Weapon.LightAttack(ComboNumber, enemiesInRange, this.gameObject);
            }
            else if (ComboType == AttackType.Secundary)
            {
                Weapon.StrongAttack(enemiesInRange, this.gameObject);
            }
        }

        public void NotifyAttackFinish()
        {
            OnAttackFinish?.Invoke();
        }

        public override void PrimaryAttack()
        {
            DoAttack(AttackType.Primary);
        }

        public override void SecundaryAttack()
        {
            DoAttack(AttackType.Secundary);
        }

        private void DoAttack(AttackType attackType)
        {
            ComboType = attackType;
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
            }
            else if (attackType == AttackType.Primary)
            {
                ComboNumber++;
                if (ComboNumber > maxCombos)
                {
                    ComboNumber = COMBO_START;
                }
            }
        }
    }
}
