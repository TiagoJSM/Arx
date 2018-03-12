using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using CommonInterfaces.Enums;
using GenericComponents.Behaviours;

namespace Assets.Standard_Assets.Weapons
{
    /*public class Grapple : MonoBehaviour, IGrapple
    {
        private ChainedProjectile _instantiatedHeldProjectile;
        private bool _attached;

        public HingeJoint2D hingePrefab;
        public float ropePartLength;
        public LayerMask layer;

        public HingeJoint2D origin;
        public ChainedProjectile projectile;

        public bool ReadyToThrow
        {
            get
            {
                return 
                    _instantiatedHeldProjectile.Status == ProjectileStatus.None ||
                    _instantiatedHeldProjectile.Status == ProjectileStatus.Stuck;
            }
        }

        public WeaponType WeaponType
        {
            get
            {
                return WeaponType.ChainedProjectile;
            }
        }

        public GameObject LeftHandWeapon
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public GameObject RightHandWeapon
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public GameObject LeftHandSocket
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public GameObject RightHandSocket
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public event Action OnAttackFinish;

        public void Spin()
        {
            throw new NotImplementedException();
        }

        public void FocusThrow()
        {
            throw new NotImplementedException();
        }

        private void Throw(float degrees, LayerMask enemyLayer, LayerMask wallMask, GameObject attacker)
        {
            if (!ReadyToThrow)
            {
                return;
            }
            StartAttack();
            _instantiatedHeldProjectile.Throw(degrees, attacker, 0);
        }

        public void Detatch()
        {
            _attached = false;
        }

        public virtual void Equipped() { }
        public virtual void Unequipped() { }

        private void StartAttack()
        {
            _attached = false;
            _instantiatedHeldProjectile.Reset();
        }

        private void CreateRopeAt(GameObject point)
        {
            var fixedJointGO = new GameObject("fixed");
            var ropeRenderer = point.AddComponent<RopeRenderer>();
            var grappleRope = point.AddComponent<GrappleRope>();

            fixedJointGO.transform.SetParent(point.transform, false);
            var fixedJoint = fixedJointGO.AddComponent<FixedJoint2D>();
            fixedJoint.autoConfigureConnectedAnchor = false;

            var distance = Vector2.Distance(point.transform.position, origin.transform.position);
            var heading = origin.transform.position - point.transform.position;
            var direction = heading / distance;

            var partsCount = (distance / ropePartLength);
            var connectedBody = fixedJointGO.GetComponent<Rigidbody2D>();

            var hinge = default(HingeJoint2D);
            for (var idx = 0; idx < partsCount; idx++)
            {
                hinge = Instantiate(hingePrefab);
                if (idx == 0)
                {
                    hinge.connectedAnchor = Vector2.zero;
                }
                hinge.transform.SetParent(point.transform, false);
                hinge.connectedBody = connectedBody;
                hinge.transform.localPosition = direction * idx * ropePartLength;
                //hinge.transform.rotation = Quaternion.Euler(0, 0, angle);
                connectedBody = hinge.GetComponent<Rigidbody2D>();
                //connectedBody.position = hinge.transform.position;
            }

            origin.connectedBody = hinge.GetComponent<Rigidbody2D>();
            origin.connectedAnchor = new Vector2(0, -ropePartLength);
            origin.enabled = true;

            grappleRope.RopeEnd = origin;
            ropeRenderer.Rope = grappleRope;
        }

        private void OnTriggerEnterHandler(Collider2D collider, ChainedProjectile projectile)
        {
            if (layer.IsInAnyLayer(collider.gameObject) && !_attached)
            {
                var point = new GameObject("point");
                point.transform.position = projectile.transform.position;
                _attached = true;
                projectile.Stop();
                CreateRopeAt(point);
                AttackFinishedHandler();
            }
        }

        void Awake()
        {
            _instantiatedHeldProjectile = Instantiate(projectile);
            _instantiatedHeldProjectile.Origin = this.gameObject;
            _instantiatedHeldProjectile.transform.parent = null;
            _instantiatedHeldProjectile.OnAttackFinish += AttackFinishedHandler;
            //_instantiatedHeldProjectile.OnTriggerEnter += OnTriggerEnterHandler;
        }

        protected void AttackFinishedHandler()
        {
            OnAttackFinish?.Invoke();
        }
    }*/
}
