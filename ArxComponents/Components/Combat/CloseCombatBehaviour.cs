using CommonInterfaces.Controllers;
using CommonInterfaces.Weapons;
using GenericComponents.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components.Combat
{
    public class CloseCombatBehaviour : BaseGenericCombatBehaviour<ICloseCombatWeapon>
    {
        private const int COMBO_START = 1;

        private List<ICharacter> _charactersAttackedOnDive;
        private Coroutine _diveAttackDetector;

        [SerializeField]
        private int maxCombos = 3;
        [SerializeField]
        private Transform _attackAreaP1;
        [SerializeField]
        private Transform _attackAreaP2;
        [SerializeField]
        private Transform _diveAttackAreaP1;
        [SerializeField]
        private Transform _diveAttackAreaP2;
        [SerializeField]
        private LayerMask _enemyLayer;

        public override event Action OnAttackFinish;

        public CloseCombatBehaviour()
        {
            _charactersAttackedOnDive = new List<ICharacter>();
        }

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
            var enemiesInRange = GetCharactersInRange(_attackAreaP1.position, _attackAreaP2.position, _enemyLayer);

            if (ComboType == AttackType.Primary)
            {
                Weapon.LightAttack(ComboNumber, enemiesInRange, this.gameObject);
            }
            else if (ComboType == AttackType.Secundary)
            {
                Weapon.StrongAttack(enemiesInRange, this.gameObject);
            }
        }

        public void StartSlashAttack()
        {
            //ToDo
        }

        public void FinishSlashAttack()
        {
            //ToDo
        }

        public override void StartDiveAttack()
        {
            base.StartDiveAttack();
            _diveAttackDetector = StartCoroutine(DiveAttackDetector());
        }

        public override void EndDiveAttack()
        {
            base.StartDiveAttack();
            if(_diveAttackDetector != null)
            {
                StopCoroutine(_diveAttackDetector);
                _diveAttackDetector = null;
            }
        }

        public void NotifyAttackFinish()
        {
            OnAttackFinish?.Invoke();
        }

        public override bool PrimaryGroundAttack()
        {
            DoGroundAttack(AttackType.Primary);
            return true;
        }

        public override bool SecundaryGroundAttack()
        {
            DoGroundAttack(AttackType.Secundary);
            return true;
        }

        public override bool PrimaryAirAttack()
        {
            DoAirAttack(AttackType.Primary);
            return true;
        }

        public override bool SecundaryAirAttack()
        {
            DoAirAttack(AttackType.Secundary);
            return true;
        }

        private void DoGroundAttack(AttackType attackType)
        {
            ComboType = attackType;
            if (attackType == AttackType.None)
            {
                ComboNumber = 0;
                AttackStyle = AttackStyle.None;
            }
            else if (attackType == AttackType.Secundary)
            {
                AttackStyle = AttackStyle.Ground;
                ComboNumber++;
                if (ComboNumber > maxCombos)
                {
                    ComboNumber = COMBO_START;
                }
            }
            else if (attackType == AttackType.Primary)
            {
                AttackStyle = AttackStyle.Ground;
                ComboNumber++;
                if (ComboNumber > maxCombos)
                {
                    ComboNumber = COMBO_START;
                }
            }
        }

        private void DoAirAttack(AttackType attackType)
        {
            ComboType = attackType;
            if (attackType == AttackType.None)
            {
                ComboNumber = 0;
                AttackStyle = AttackStyle.None;
            }
            else
            {
                ComboNumber = COMBO_START;
                AttackStyle = AttackStyle.Aerial;
            }
        }

        private IEnumerator DiveAttackDetector()
        {
            while (true)
            {
                var enemiesInRange = GetCharactersInRange(_diveAttackAreaP1.position, _diveAttackAreaP2.position, _enemyLayer);
                Weapon.DiveAttack(enemiesInRange, this.gameObject);
                yield return null;
            }
        }
    }
}
