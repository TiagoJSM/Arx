using MathHelper;
using MathHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Combat
{
    public class AimBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject _aimingArm;
        [SerializeField]
        private GameObject _head;
        [SerializeField]
        [Range(0, 90)]
        private float _aimArmLimit = 90;
        [SerializeField]
        [Range(0, 90)]
        private float _headLookLimit = 90;

        public float AimAngle { get; set; }

        public float GetWeaponAimAngle()
        {
            var degrees = AimAngle;
            var radians = degrees * Mathf.Deg2Rad;
            var quadrant = radians.GetQuadrant();

            var inverted = (_aimingArm.transform.lossyScale.x < 0) || (_aimingArm.transform.lossyScale.y < 0);

            if (!inverted)
            {
                degrees = quadrant == 3 ? degrees - FloatUtils.FullDegreeTurn : degrees;
                degrees = Mathf.Clamp(degrees, -_aimArmLimit, _aimArmLimit);
            }
            else
            {
                degrees = Mathf.Clamp(degrees, FloatUtils.HalfDegreeTurn - _aimArmLimit, FloatUtils.HalfDegreeTurn + _aimArmLimit);
            }
            return degrees;
        }

        private void LateUpdate()
        {
            AimAtTarget(_head, _headLookLimit);
            AimAtTarget(_aimingArm, _aimArmLimit);
        }

        private void AimAtTarget(GameObject obj, float limit)
        {
            var rotation = AimAtTargetRotation(limit);
            obj.transform.rotation = Quaternion.Euler(0, 0, rotation);
        }

        private float AimAtTargetRotation(float limit)
        {
            var inverted = (_aimingArm.transform.lossyScale.x < 0) || (_aimingArm.transform.lossyScale.y < 0);

            var rotation = default(float);
            if (!inverted)
            {
                rotation = AimAngle;
            }
            else
            {
                rotation = AimAngle - 180;
            }
            if (rotation > 180)
            {
                rotation -= 360;
            }
            rotation = Mathf.Clamp(rotation, -limit, limit);
            return rotation;
        }
    }
}
