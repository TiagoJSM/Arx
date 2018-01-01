using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonInterfaces.Weapons;
using UnityEngine;
using CommonInterfaces.Controllers;

namespace Assets.Standard_Assets.Weapons
{
    [CreateAssetMenu(fileName = "Fists", menuName = "Weapons/Create Fist Weapon", order = 1)]
    public class Fists : BaseCloseCombatWeapon
    {
        [SerializeField]
        private int _lightCombo1Damage;
        [SerializeField]
        private int _lightCombo2Damage;
        [SerializeField]
        private int _lightCombo3Damage;
        [SerializeField]
        private int _strongComboDamage;
        [SerializeField]
        private int _diveDamage;

        public Fists()
        {
            WeaponType = WeaponType.Fist;
        }

        public override void LightAttack(int comboCount, IEnumerable<ICharacter> targets, GameObject attacker)
        {
            var damage = GetLightAttackDamage(comboCount);
            DealDamage(damage, targets, attacker, DamageType.Fist, AttackTypeDetail.GroundLight, comboCount);
        }

        public override void StrongAttack(int comboCount, IEnumerable<ICharacter> targets, GameObject attacker)
        {
            DealDamage(_strongComboDamage, targets, attacker, DamageType.Fist, AttackTypeDetail.GroundStrong, comboCount);
        }

        public override void DiveAttack(IEnumerable<ICharacter> targets, GameObject attacker)
        {
            DealDamage(_diveDamage, targets, attacker, DamageType.Fist, AttackTypeDetail.AirStrong);
        }

        private int GetLightAttackDamage(int comboCount)
        {
            switch (comboCount)
            {
                case 1:
                    return _lightCombo1Damage;
                case 2:
                    return _lightCombo2Damage;
                case 3:
                    return _lightCombo3Damage;
                default:
                    return 0;
            }
        }
    }
}
