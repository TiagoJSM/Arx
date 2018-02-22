using Assets.Standard_Assets.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Combat
{
    public interface IThrowWeapon : IWeapon
    {
        bool Throw(Vector3 origin, float angleInDegrees, LayerMask enemyLayer, GameObject attacker, float power);
    }
}
