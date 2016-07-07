using CommonInterfaces.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonInterfaces.Weapons
{
    public enum WeaponType
    {
        Sword,
        Fist,
        Shoot,
        ChainedProjectile
    }

    public interface IWeapon
    {
        WeaponType WeaponType { get; }
        //void StartStrongAttack();
        //void StartLightAttack(int comboCount);
        //void AttackIsOver();
    }
}
