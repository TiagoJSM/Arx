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
        public GameObject aimingArm;
        public GameObject head;
        [Range(0, 90)]
        public float aimLimit = 90;
        [Range(0, 90)]
        public float headLookLimit = 90;

        public override event Action OnAttackFinish;

        public override bool PrimaryAttack()
        {
            var degrees = GetWeaponAimAngle();
            return Weapon.Shoot(degrees);
        }

        public override bool SecundaryAttack()
        {
            throw new NotImplementedException();
        }

        void Awake()
        {
            this.enabled = false;
        }

        void Update()
        {
            AimAtTarget(head, headLookLimit);
            AimAtTarget(aimingArm, aimLimit);
            OnAttackFinish?.Invoke();
        }

        private float AimAtTargetRotation(float limit)
        {
            var center = aimingArm.transform.position;
            var aimPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var inverted = (aimingArm.transform.lossyScale.x < 0) || (aimingArm.transform.lossyScale.y < 0);

            var rotation = default(float);
            if (!inverted)
            {
                rotation = FloatUtils.AngleBetween(center, aimPosition);
            }
            else
            {
                rotation = (-FloatUtils.AngleBetween(center, aimPosition) + 180);
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
            var center = aimingArm.transform.position;
            var aimPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var degrees = FloatUtils.AngleBetween(center, aimPosition).ReduceToSingleTurn();
            var radians = degrees * Mathf.Deg2Rad;
            var quadrant = radians.GetQuadrant();

            //ToDo: direction should be provided by ICombatComponent
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
