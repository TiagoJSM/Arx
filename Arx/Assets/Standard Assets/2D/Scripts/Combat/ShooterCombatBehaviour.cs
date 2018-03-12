using Assets.Standard_Assets.Weapons;
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
    [SerializeField]
    private Transform _origin;

    public bool Shoot(float aimAngle)
    {
        return Weapon.Shoot(aimAngle, _enemyLayer, this.gameObject, _origin.position);
    }
}

