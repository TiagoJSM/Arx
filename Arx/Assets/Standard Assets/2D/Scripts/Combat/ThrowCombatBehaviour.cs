using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Combat
{
    public class ThrowCombatBehaviour : BaseGenericCombatBehaviour<IThrowWeapon>
    {
        [SerializeField]
        private LayerMask _enemyLayer;
        [SerializeField]
        private float _standardThrowPower = 35;
        [SerializeField]
        private Transform _origin;

        public void Throw(float angle)
        {
            Weapon.Throw(_origin.position, angle, _enemyLayer, gameObject, _standardThrowPower);
        }
    }
}
