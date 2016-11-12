using CommonInterfaces.Enums;
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
    public class ChainThrowCombatBehaviour : BaseGenericCombatBehaviour<IChainThrowWeapon>
    {
        private bool _performingAnimation;

        private IChainThrowWeapon _weapon;
        private float _armRotation;

        [SerializeField]
        private LayerMask _enemyLayer;
        [Range(0, 90)]
        public float aimLimit = 90;
        public GameObject aimingArm;

        public override IChainThrowWeapon Weapon
        {
            get
            {
                return _weapon;
            }
            set
            {
                if(_weapon == value)
                {
                    return;
                }
                if(_weapon != null)
                {
                    _weapon.OnAttackFinish -= OnAttackFinishHandler;
                }
                _weapon = value;
                if (_weapon != null)
                {
                    _weapon.OnAttackFinish += OnAttackFinishHandler;
                }
            }
        }
        public override event Action OnAttackFinish;

        public override bool PrimaryGroundAttack()
        {
            return !_performingAnimation;
        }

        public override bool SecundaryGroundAttack()
        {
            return !_performingAnimation;
        }
        public override bool PrimaryAirAttack()
        {
            return !_performingAnimation;
        }

        public override bool SecundaryAirAttack()
        {
            return !_performingAnimation;
        }

        public override bool ChargeAttack()
        {
            return !_performingAnimation;
        }

        public override bool ReleaseChargeAttack()
        {
            return true;
        }

        public void ThrowChain()
        {
            aimingArm.transform.rotation = Quaternion.Euler(0, 0, _armRotation);
            var degrees = GetWeaponAimAngle();
            Weapon.Throw(degrees, _enemyLayer, this.gameObject);
        }

        public void PerformingChainThrowAnimation(bool performing)
        {
            _performingAnimation = performing;
        }

        void Awake()
        {
            this.enabled = false;
        }

        void LateUpdate()
        {
            if (_performingAnimation)
            {
                aimingArm.transform.rotation = Quaternion.Euler(0, 0, _armRotation);
            }
            else
            {
                AimAtTarget(aimingArm, aimLimit);
            }
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
            _armRotation = AimAtTargetRotation(limit);
            obj.transform.rotation = Quaternion.Euler(0, 0, _armRotation);
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

        private void OnAttackFinishHandler()
        {
            OnAttackFinish?.Invoke();
        }
    }
}
