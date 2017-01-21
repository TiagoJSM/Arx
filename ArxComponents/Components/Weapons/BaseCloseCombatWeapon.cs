using CommonInterfaces.Controllers;
using CommonInterfaces.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components.Weapons
{
    public abstract class BaseCloseCombatWeapon : BaseWeapon, ICloseCombatWeapon
    {
        public abstract void LightAttack(int comboCount, IEnumerable<ICharacter> targets, GameObject attacker);
        public abstract void StrongAttack(int comboCount, IEnumerable<ICharacter> targets, GameObject attacker);
        public abstract void DiveAttack(IEnumerable<ICharacter> targets, GameObject attacker);

        protected void DealDamage(
            int damage, 
            IEnumerable<ICharacter> targets, 
            GameObject attacker,
            DamageType damageType,
            AttackTypeDetail attackType,
            int comboNumber = 1)
        {
            foreach (var enemy in targets)
            {
                enemy.Attacked(attacker, damage, null, damageType, attackType, comboNumber);
            }
        }
    }
}
