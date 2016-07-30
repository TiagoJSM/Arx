using CommonInterfaces.Enums;
using CommonInterfaces.Weapons;
using Extensions;
using GenericComponents.Animation.Playables;
using GenericComponents.Containers;
using GenericComponents.Controllers.Interaction.Environment;
using GenericComponents.Enums;
using GenericComponents.Helpers;
using GenericComponents.Interfaces.States.PlatformerCharacter;
using GenericComponents.StateMachine;
using GenericComponents.StateMachine.States.PlatformerCharacter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.Director;

namespace GenericComponents.Controllers.Characters
{
    [RequireComponent(typeof(LedgeChecker))]
    [RequireComponent(typeof(RoofChecker))]
    public class PlatformerCharacterController : BasePlatformerController
    {
        private bool _grabbingLedge = false;
        [SerializeField]
        private Direction _direction;

        private LedgeChecker _ledgeChecker;
        private RoofChecker _roofChecker;
        private Rigidbody2D _rigidBody;
        private float _gravityScale;
        private Collider2D _detectedLedge;
        private Collider2D _lastGrabbedLedge;
        private bool _ledgeDetected;

        public float groundMovementForce = 2f;
        public float airMovementForce = 1f;
        public float maxRunSpeed = 6.0f;
        public float airMaxSpeed = 2.0f;
        public float jumpForce = 700.0f;
        public Collider2D[] standingColliders;
        public Collider2D[] duckingColliders;
        public float maxRollSpeed = 12.0f;

        public Direction Direction
        { 
            get
            {
                return _direction;
            }
        }

        private bool DetectingPreviousGrabbedLedge
        {
            get
            {
                if(_lastGrabbedLedge == null)
                {
                    return false;
                }
                return _lastGrabbedLedge == _detectedLedge;
            }
        }

        public bool CanGrabLedge
        {
            get
            {
                return _ledgeDetected && !IsGrounded && !DetectingPreviousGrabbedLedge && !_grabbingLedge;
            }
        }

        public bool IsGrounded { get; private set; }

        public bool CanStand { get; private set; }

        public float VerticalSpeed
        {
            get
            {
                return _rigidBody.velocity.y;
            }
        }

        public float HorizontalSpeed
        {
            get
            {
                return _rigidBody.velocity.x;
            }
        }

        public bool GrabbingLedge
        {
            get
            {
                return _grabbingLedge;
            }
        }

        public void LedgeDetected(bool detected, Collider2D ledgeCollider)
        {
            _ledgeDetected = detected;
            if (!detected)
            {
                if (_grabbingLedge)
                {
                    DropLedge();
                }
                _lastGrabbedLedge = null;
                return;
            }
            _detectedLedge = ledgeCollider;
        }

        public void DoMove(float move)
        {
            _direction = DirectionOfMovement(move, _direction);
            var movementForce = IsGrounded ? groundMovementForce : airMovementForce;

            if (Math.Abs(move) < 0.5)
            {
                _rigidBody.velocity = new Vector2(0, _rigidBody.velocity.y);
            }
            else
            {
                _rigidBody.AddForce(new Vector2(movementForce, 0) * DirectionValue(_direction), ForceMode2D.Impulse);
            }
            
            Flip(_direction);
        }

        public void DoGrabLedge()
        {
            if (_detectedLedge == null)
            {
                return;
            }
            _lastGrabbedLedge = _detectedLedge;
            transform.parent = _lastGrabbedLedge.gameObject.transform;
            _rigidBody.gravityScale = 0;
            _rigidBody.velocity = Vector2.zero;
            _grabbingLedge = true;
        }

        public void JumpUp()
        {
            _rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        public void DropLedge()
        {
            _grabbingLedge = false;
            transform.parent = null;
            _rigidBody.gravityScale = _gravityScale;
        }

        public virtual void Duck()
        {
            //_comboNumber = 0;
            foreach(var duckCollider in duckingColliders)
            {
                duckCollider.enabled = true;
            }
            foreach (var standingCollider in standingColliders)
            {
                standingCollider.enabled = false;
            }
        }

        public virtual void Stand()
        {
            //_comboNumber = 0;
            foreach (var duckCollider in duckingColliders)
            {
                duckCollider.enabled = false;
            }
            foreach (var standingCollider in standingColliders)
            {
                standingCollider.enabled = true;
            }
        }

        public void StayStill()
        {
            _rigidBody.velocity = new Vector2(0, _rigidBody.velocity.y);
        }

        public void Roll(float move)
        {
            _direction = DirectionOfMovement(move, _direction);
            Flip(_direction);
            var direction = DirectionValue(_direction);
            _rigidBody.velocity = new Vector2(direction * maxRollSpeed, _rigidBody.velocity.y);
        }

        protected virtual void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _gravityScale = _rigidBody.gravityScale;
            _ledgeChecker = GetComponent<LedgeChecker>();
            _roofChecker = GetComponent<RoofChecker>();
        }

        protected virtual void FixedUpdate()
        {
            Collider2D collider;
            var ledgeDetected = _ledgeChecker.IsLedgeDetected(out collider);
            LedgeDetected(ledgeDetected, collider);
            IsGrounded = CheckGrounded();
            CanStand = !_roofChecker.IsTouchingRoof;
            //ToDo, this seems wrong
            //check https://vonlehecreative.wordpress.com/2010/02/02/unity-resource-velocitylimiter/
            var currentMaxSpeed = IsGrounded ? maxRunSpeed : airMaxSpeed;
            _rigidBody.velocity = new Vector2(Mathf.Clamp(_rigidBody.velocity.x, -currentMaxSpeed, currentMaxSpeed), _rigidBody.velocity.y);
        }

        void OnDrawGizmosSelected()
        {
            base.DrawGizmos();
        }
    }
}
