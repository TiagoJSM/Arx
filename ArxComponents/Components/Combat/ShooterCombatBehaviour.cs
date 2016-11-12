using CommonInterfaces.Weapons;
using MathHelper;
using MathHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components.Combat
{
    public class ShooterCombatBehaviour : BaseGenericCombatBehaviour<IShooterWeapon>
    {
        [SerializeField]
        private LayerMask _enemyLayer;

        public GameObject aimingArm;
        public GameObject head;
        [Range(0, 90)]
        public float aimLimit = 90;
        [Range(0, 90)]
        public float headLookLimit = 90;

        public override event Action OnAttackFinish;

        public override bool PrimaryGroundAttack()
        {
            return PrimaryAttack();
        }

        public override bool PrimaryAirAttack()
        {
            return PrimaryAttack();
        }

        void Awake()
        {
            this.enabled = false;
        }

        private void LateUpdate()
        {
            AimAtTarget(head, headLookLimit);
            AimAtTarget(aimingArm, aimLimit);
            OnAttackFinish?.Invoke();
        }

        private bool PrimaryAttack()
        {
            AimAtTarget(aimingArm, aimLimit);
            var degrees = GetWeaponAimAngle();
            return Weapon.Shoot(degrees, _enemyLayer, this.gameObject);
        }

        private float AimAtTargetRotation(float limit)
        {
            var inverted = (aimingArm.transform.lossyScale.x < 0) || (aimingArm.transform.lossyScale.y < 0);

            var rotation = default(float);
            if (!inverted)
            {
                rotation = AimAngle;
            }
            else
            {
                rotation = (-AimAngle) + 180;
            }
            if (rotation > 180)
            {
                rotation -= 360;
            }
            rotation = Mathf.Clamp(rotation, -limit, limit);
            return rotation;
        }

        private void AimAtTarget(GameObject obj, float limit)
        {
            var rotation = AimAtTargetRotation(limit);
            obj.transform.rotation = Quaternion.Euler(0, 0, rotation);
        }

        private float GetWeaponAimAngle()
        {
            var degrees = AimAngle;
            var radians = degrees * Mathf.Deg2Rad;
            var quadrant = radians.GetQuadrant();

            var inverted = (aimingArm.transform.lossyScale.x < 0) || (aimingArm.transform.lossyScale.y < 0);

            if (!inverted)
            {
                degrees = quadrant == 3 ? degrees - FloatUtils.FullDegreeTurn : degrees;
                degrees = Mathf.Clamp(degrees, -aimLimit, aimLimit);
            }
            else
            {
                degrees = Mathf.Clamp(degrees, FloatUtils.HalfDegreeTurn - aimLimit, FloatUtils.HalfDegreeTurn + aimLimit);
            }
            return degrees;
        }
    }
}
