using GenericComponentsCharacters = Assets.Standard_Assets._2D.Scripts.Controllers;
using MathHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Standard_Assets.Weapons;
using Assets.Standard_Assets.Extensions;

namespace Assets.Standard_Assets.Characters.Enemies.Wall_Shooter.Scripts
{
    public class WallShooterCharacterController : GenericComponentsCharacters.BasePlatformerController
    {
        [SerializeField]
        private Projectile _projectilePrefab;
        [SerializeField]
        private LayerMask _enemyLayer;
        [SerializeField]
        private Transform _projectileSpawnPosition;
        [SerializeField]
        private AudioSource[] _shootSounds;

        public void Shoot()
        {
            var euler = this.transform.rotation.eulerAngles;
            var transformed = transform.TransformVector(Vector3.right);
            var angle = Vector2.Angle(Vector2.right, transformed);
            var cross = Vector3.Cross(Vector2.right, transformed);
            angle = cross.z < 0 ? 360f - angle : angle;
            var direction = angle.GetDirectionVectorFromDegreeAngle();
            var position = _projectileSpawnPosition != null ? _projectileSpawnPosition.position : transform.position;
            var projectile = Instantiate(_projectilePrefab, position, transform.rotation);
            projectile.Direction = direction;
            projectile.Attacker = this.gameObject;
            projectile.EnemyLayer = _enemyLayer;
            projectile.Damage = 1;
            PlayShootSound();
        }

        private void PlayShootSound()
        {
            _shootSounds.PlayRandom();
        }
    }
}
