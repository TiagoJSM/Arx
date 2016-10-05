using CommonInterfaces.Weapons;
using MathHelper;
using MathHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components.Weapons
{
    public class Shooter : MonoBehaviour, IShooterWeapon
    {
        //private float _lastShotTime;
        [SerializeField]
        private float _cooldown = 1;

        public Transform t;
        public Projectile projectilePrefab;

        public event Action OnCooldownFinish;

        public bool InCooldown
        {
            get
            {
                return RemainingCooldownTime > 0;
            }
        }

        public float RemainingCooldownTime { get; private set; }

        public WeaponType WeaponType
        {
            get
            {
                return WeaponType.Shoot;
            }
        }

        public bool Shoot(float angleInDegrees)
        {
            if(InCooldown)
            {
                return false;
            }
            var direction = angleInDegrees.GetDirectionVectorFromDegreeAngle();
            var projectile = Instantiate(projectilePrefab);
            projectile.transform.position = this.transform.position;
            projectile.direction = direction;
            RemainingCooldownTime = _cooldown;

            return true;
        }

        void Awake()
        {
            //_lastShotTime = Time.time - _cooldown;
        }

        void Update()
        {
            if (!InCooldown)
            {
                return;
            }
            //var elapsed = Time.time - _lastShotTime;
            RemainingCooldownTime =  Mathf.Max(RemainingCooldownTime - Time.deltaTime, 0);
            if (!InCooldown)
            {
                OnCooldownFinish?.Invoke();
            }
        }
    }
}
