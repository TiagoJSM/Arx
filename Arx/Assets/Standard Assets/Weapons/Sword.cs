using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using CommonInterfaces.Controllers;
using Assets.Standard_Assets.Scripts;

namespace Assets.Standard_Assets.Weapons
{
    [CreateAssetMenu(fileName = "Sword", menuName = "Weapons/Create Sword Weapon", order = 1)]
    public class Sword : BaseCloseCombatWeapon
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

        public Sword()
        {
            WeaponType = WeaponType.Sword;
        }

        public override void LightAttack(int comboCount, IEnumerable<ICharacter> targets, GameObject attacker)
        {
            var damage = GetLightAttackDamage(comboCount);
            DealDamage(damage, targets, attacker, DamageType.Sword, AttackTypeDetail.GroundLight, comboCount);
        }

        public override void StrongAttack(int comboCount, IEnumerable<ICharacter> targets, GameObject attacker)
        {
            DealDamage(_strongComboDamage, targets, attacker, DamageType.Sword, AttackTypeDetail.GroundStrong, comboCount);
        }

        public override void DiveAttack(IEnumerable<ICharacter> targets, GameObject attacker)
        {
            DealDamage(_diveDamage, targets, attacker, DamageType.Sword, AttackTypeDetail.AirStrong);
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
