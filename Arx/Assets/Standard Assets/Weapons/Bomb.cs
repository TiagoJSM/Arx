using Assets.Standard_Assets._2D.Scripts.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Weapons
{
    [CreateAssetMenu(fileName = "Bomb", menuName = "Weapons/Create Bomb Weapon", order = 1)]
    public class Bomb : BaseWeapon, IThrowWeapon
    {
        [SerializeField]
        private float _range = 1;
        [SerializeField]
        private int _maxRangeDamage = 6;
        [SerializeField]
        private int _minRangeDamage = 2;

        public bool Shoot(float angleInDegrees, LayerMask enemyLayer, GameObject attacker, float power)
        {
            throw new NotImplementedException();
        }
    }
}
