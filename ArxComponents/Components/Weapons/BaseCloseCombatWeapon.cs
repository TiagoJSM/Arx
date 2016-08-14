using CommonInterfaces.Controllers;
using CommonInterfaces.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components.Weapons
{
    public abstract class BaseCloseCombatWeapon : MonoBehaviour, ICloseCombatWeapon
    {
        public WeaponType WeaponType { get; protected set; }

        public abstract void LightAttack(int comboCount, IEnumerable<ICharacter> targets, GameObject attacker);
        public abstract void StrongAttack(IEnumerable<ICharacter> targets, GameObject attacker);

        protected void DealDamage(int damage, IEnumerable<ICharacter> targets, GameObject attacker)
        {
            foreach (var enemy in targets)
            {
                enemy.Attacked(attacker, damage, null);
            }
        }
    }
}
