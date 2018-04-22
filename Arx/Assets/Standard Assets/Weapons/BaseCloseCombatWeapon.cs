using Assets.Standard_Assets.Scripts;
using CommonInterfaces.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Weapons
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
            for(var idx = 0; idx < targets.Count(); idx++)
            {
                var enemy = targets.ElementAt(idx);
                if (enemy.CanBeAttacked)
                {
                    enemy.Attacked(attacker, damage, null, damageType, attackType, comboNumber);
                }
            }
        }
    }
}
