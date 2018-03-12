using CommonInterfaces.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Weapons
{
    public interface ICloseCombatWeapon : IWeapon
    {
        void StrongAttack(int comboCount, IEnumerable<ICharacter> targets, GameObject attacker);
        void LightAttack(int comboCount, IEnumerable<ICharacter> targets, GameObject attacker);
        void DiveAttack(IEnumerable<ICharacter> targets, GameObject attacker);
    }
}
