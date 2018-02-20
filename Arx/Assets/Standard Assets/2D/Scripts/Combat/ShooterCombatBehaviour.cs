using CommonInterfaces.Weapons;
using MathHelper;
using MathHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ShooterCombatBehaviour : BaseGenericCombatBehaviour<IShooterWeapon>
{
    [SerializeField]
    private LayerMask _enemyLayer;

    public bool Shoot(float aimAngle)
    {
        return Weapon.Shoot(aimAngle, _enemyLayer, this.gameObject);
    }
}

