using CommonInterfaces.Weapons;
using MathHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components.Weapons
{
    public class Shooter : MonoBehaviour, IWeapon
    {
        private float _lastShotTime;
        [SerializeField]
        private float _cooldown = 1;

        public Transform t;
        public Projectile projectilePrefab;

        public bool Shoot(float angleInDegrees)
        {
            var elapsed = Time.time - _lastShotTime;
            if(elapsed < _cooldown)
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

        public void AttackIsOver()
        {
            //throw new NotImplementedException();
        }

        public void StartLightAttack(int comboCount)
        {
            throw new NotImplementedException();
        }

        public void StartStrongAttack()
        {
            //throw new NotImplementedException();
        }

        void Awake()
        {
            _lastShotTime = Time.time - _cooldown;
        }

        void Update()
        {
            var angle = FloatUtils.AngleBetween(this.transform.position, t.position);
            Shoot(angle);
        }
    }
}
