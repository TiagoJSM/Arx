using MathHelper;
using MathHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Weapons
{
    [CreateAssetMenu(fileName = "Shooter", menuName = "Weapons/Create Shooter Weapon", order = 1)]
    public class Shooter : BaseWeapon, IShooterWeapon
    {
        private float _lastShotTime;

        [SerializeField]
        private float _cooldown = 1;
        [SerializeField]
        private int _damage = 10;

        public Projectile projectilePrefab;

        public event Action OnCooldownFinish;

        public bool InCooldown
        {
            get
            {
                return RemainingCooldownTime > 0;
            }
        }

        public float RemainingCooldownTime
        {
            get
            {
                var elapsed = Time.time - _lastShotTime;
                if(elapsed > _cooldown)
                {
                    return 0;
                }
                return _cooldown - elapsed;
            }
        }

        public Shooter()
        {
            WeaponType = WeaponType.Shoot;
        }

        public bool Shoot(float angleInDegrees, LayerMask enemyLayer, GameObject attacker, Vector3 origin)
        {
            if(InCooldown)
            {
                return false;
            }
            var direction = angleInDegrees.GetDirectionVectorFromDegreeAngle();
            var projectile = Instantiate(projectilePrefab);
            projectile.transform.position = origin;
            projectile.Direction = direction;
            projectile.Attacker = attacker;
            projectile.EnemyLayer = enemyLayer;
            projectile.Damage = _damage;
            _lastShotTime = Time.time;

            return true;
        }
    }
}
