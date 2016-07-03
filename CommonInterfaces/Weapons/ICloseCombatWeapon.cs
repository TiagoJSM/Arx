using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonInterfaces.Weapons
{
    public interface ICloseCombatWeapon : IWeapon
    {
        void StartStrongAttack();
        void StartLightAttack(int comboCount);
        void AttackIsOver();
    }
}
