using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Combat
{
    public interface IThrowWeapon
    {
        bool Shoot(float angleInDegrees, LayerMask enemyLayer, GameObject attacker, float power);
    }
}
