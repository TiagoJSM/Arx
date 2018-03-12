using Assets.Standard_Assets._2D.Scripts.Combat;
using MathHelper.Extensions;
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
        private BombBehaviour _bombPrefab;

        public Bomb()
        {
            WeaponType = WeaponType.Throw;
        }

        public bool Throw(Vector3 origin, float angleInDegrees, LayerMask enemyLayer, GameObject attacker, float power)
        {
            var direction = angleInDegrees.GetDirectionVectorFromDegreeAngle();
            var bomb = Instantiate(_bombPrefab);
            bomb.transform.position = origin;
            bomb.Throw(direction * power, enemyLayer, attacker);
            return true;
        }
    }
}
