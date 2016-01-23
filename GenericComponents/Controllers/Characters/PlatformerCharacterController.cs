﻿using GenericComponents.Animation.Playables;
using GenericComponents.Containers;
using GenericComponents.Controllers.Interaction.Environment;
using GenericComponents.Enums;
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
    public class PlatformerCharacterController : BasePlatformerController, IPlatformerCharacterController
    {
        private bool _grabbingLedge = false;
        [SerializeField]
        private Direction _direction;
        private StateManager<IPlatformerCharacterController, PlatformerCharacterAction> _stateManager;

        private LedgeChecker _ledgeChecker;
        private RoofChecker _roofChecker;
        private Animator _animator;
        private Rigidbody2D _rigidBody;
        private float _gravityScale;
        private Collider2D _detectedLedge;
        private Collider2D _lastGrabbedLedge;
        private bool _ledgeDetected;

        private float _move;
        private float _vertical;
        private bool _jump;

        public float maxRunSpeed = 6.0f;
        public float airMaxSpeed = 2.0f;
        public float jumpForce = 700.0f;
        public Collider2D[] standingColliders;
        public Collider2D[] duckingColliders;
        public float maxRollSpeed = 12.0f;
        public float rollingDuration = 1;

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

        [SerializeField]
        public PlatformerCharacterAnimations animations;

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

        public bool GrabbingLedge
        {
            get
            {
                return _grabbingLedge;
            }
        }

        void Start()
        {
            _animator = GetComponent<Animator>();
            _rigidBody = GetComponent<Rigidbody2D>();
            _gravityScale = _rigidBody.gravityScale;
            _ledgeChecker = GetComponent<LedgeChecker>();
            _roofChecker = GetComponent<RoofChecker>();
            _stateManager = new PlatformerCharacterStateManager(this, rollingDuration);
            _animator.Play(new PlatformerCharacterAnimationPlayable(_stateManager, animations, rollingDuration));
        }

        void FixedUpdate()
        {
            Collider2D collider;
            var ledgeDetected = _ledgeChecker.IsLedgeDetected(out collider);
            LedgeDetected(ledgeDetected, collider);
            IsGrounded = CheckGrounded();
            CanStand = !_roofChecker.IsTouchingRoof;
            var action = new PlatformerCharacterAction(_move, _vertical, _jump);
            _stateManager.Perform(action);
            _move = 0;
            _vertical = 0;
            _jump = false;
        }

        public void Move(float move, float vertical, bool jump)
        {
            _move = move;
            _vertical = vertical;
            _jump = jump;
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
            _rigidBody.velocity = new Vector2(move * maxRunSpeed, _rigidBody.velocity.y);
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
            _rigidBody.AddForce(new Vector2(0, jumpForce));
        }

        public void DropLedge()
        {
            _grabbingLedge = false;
            transform.parent = null;
            _rigidBody.gravityScale = _gravityScale;
        }

        public void Duck()
        {
            foreach(var duckCollider in duckingColliders)
            {
                duckCollider.enabled = true;
            }
            foreach (var standingCollider in standingColliders)
            {
                standingCollider.enabled = false;
            }
        }

        public void Stand()
        {
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

        void OnDrawGizmosSelected()
        {
            base.DrawGizmos();
        }
    }
}