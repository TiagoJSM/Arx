using CommonInterfaces.Weapons;
using MathHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components.Weapons
{
    public class Shooter : MonoBehaviour, IShooterWeapon
    {
        private float _lastShotTime;
        [SerializeField]
        private float _cooldown = 1;

        public Transform t;
        public Projectile projectilePrefab;

        public bool InCooldown
        {
            get
            {
                return !(RemainingCooldownTime > 0);
            }
        }

        public float RemainingCooldownTime
        {
            get
            {
                var elapsed = Time.time - _lastShotTime;
                return Mathf.Max(elapsed - _cooldown, 0);
            }
        }

        public bool Shoot(float angleInDegrees)
        {
            if(InCooldown)
            {
                return false;
            }
            var angleInRadians = angleInDegrees * Mathf.Deg2Rad;
            var direction = new Vector3(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));
            var projectile = Instantiate(projectilePrefab);
            projectile.transform.position = this.transform.position;
            projectile.direction = direction;
            _lastShotTime = Time.time;

            return true;
        }

        void Awake()
        {
            _lastShotTime = Time.time - _cooldown;
        }
    }
}
