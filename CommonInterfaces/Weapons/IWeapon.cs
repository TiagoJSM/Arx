using CommonInterfaces.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
        CommonVisualWeaponComponent LeftHandWeapon { get; }
        CommonVisualWeaponComponent RightHandWeapon { get; }
        GameObject LeftHandSocket { get; set; }
        GameObject RightHandSocket { get; set; }

        void Equipped();
        void Unequipped();
    }
}
