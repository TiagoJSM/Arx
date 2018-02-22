using CommonInterfaces.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Weapons
{
    public enum WeaponType
    {
        Sword,
        Fist,
        Shoot,
        ChainedProjectile,
        Throw
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
