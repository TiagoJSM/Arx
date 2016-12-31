using ArxGame.Components.Weapons;
using GenericComponents.Controllers.Characters;
using MathHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Enemies.Wall_Shooter.Scripts
{
    public class WallShooterCharacterController : GenericComponents.Controllers.Characters.CharacterController
    {
        [SerializeField]
        private Projectile _projectilePrefab;
        [SerializeField]
        private LayerMask _enemyLayer;

        public void Shoot()
        {
            var euler = this.transform.rotation.eulerAngles;
            var transformed = transform.TransformVector(Vector3.right);
            var angle = Vector2.Angle(Vector2.right, transformed);
            var cross = Vector3.Cross(Vector2.right, transformed);
            angle = cross.z < 0 ? 360f - angle : angle;
            var direction = angle.GetDirectionVectorFromDegreeAngle();
            var projectile = Instantiate(_projectilePrefab);
            projectile.transform.position = this.transform.position;
            projectile.direction = direction;
            projectile.Attacker = this.gameObject;
            projectile.EnemyLayer = _enemyLayer;
            projectile.Damage = 1;
        }
    }
}
