﻿using CommonInterfaces.Enums;
using Extensions;
using GenericComponents.Animation.Playables;
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
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(LedgeChecker))]
    [RequireComponent(typeof(RoofChecker))]
    public class PlatformerCharacterController : BasePlatformerController, IPlatformerCharacterController
    {
        private const int MAX_COMBOS = 3;
        private const int COMBO_START = 1;

        private bool _grabbingLedge = false;
        [SerializeField]
        private Direction _direction;
        private StateManager<IPlatformerCharacterController, PlatformerCharacterAction> _stateManager;
        private PlatformerCharacterAnimationPlayable _characterAnimations;
        private AttackType? _comboType;
        private int _comboNumber;
        private PlatformerCharacterStateAnimationPlayable _statePlayable;

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
        private AttackType _attackAction;

        public float groundMovementForce = 2f;
        public float airMovementForce = 1f;
        public float maxRunSpeed = 6.0f;
        public float airMaxSpeed = 2.0f;
        public float jumpForce = 700.0f;
        public Collider2D[] standingColliders;
        public Collider2D[] duckingColliders;
        public float maxRollSpeed = 12.0f;
        public float rollingDuration = 1;

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

        public bool IsCurrentAnimationOver
        {
            get
            {
                return _statePlayable.NormalizedTime >= 1;
            }
        }

        public int ComboNumber
        {
            get
            {
                return _comboNumber;
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
            _statePlayable = new PlatformerCharacterStateAnimationPlayable(_stateManager, animations, rollingDuration);
            _characterAnimations = new PlatformerCharacterAnimationPlayable(_statePlayable);
            
            _animator.Play(_characterAnimations);
            //var cross = new CrossFadePlayable();
            //cross.Play(new AnimationClipPlayable(animations.runningAnimation));
            //_animator.Play(cross);
        }

        void FixedUpdate()
        {
            Collider2D collider;
            var ledgeDetected = _ledgeChecker.IsLedgeDetected(out collider);
            LedgeDetected(ledgeDetected, collider);
            IsGrounded = CheckGrounded();
            CanStand = !_roofChecker.IsTouchingRoof;
            var action = new PlatformerCharacterAction(_move, _vertical, _jump, _attackAction);
            _stateManager.Perform(action);
            _move = 0;
            _vertical = 0;
            _jump = false;
            //_attackAction = AttackType.None;
            //ToDo, this seems wrong
            //check https://vonlehecreative.wordpress.com/2010/02/02/unity-resource-velocitylimiter/
            var currentMaxSpeed = IsGrounded ? maxRunSpeed : airMaxSpeed;
            _rigidBody.velocity = new Vector2(Mathf.Clamp(_rigidBody.velocity.x, -currentMaxSpeed, currentMaxSpeed), _rigidBody.velocity.y);
        }

        public void Move(float move, float vertical, bool jump)
        {
            _move = move;
            _vertical = vertical;
            _jump = jump;
        }

        public void LightAttack()
        {
            _attackAction = AttackType.Light;
            //if(_statePlayable.TimeToFinish < 1)
            //{
            //    _attackAction = AttackType.Light;
            //}
        }

        public void StrongAttack()
        {
            _attackAction = AttackType.Strong;
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

        public void Duck()
        {
            _comboNumber = 0;
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
            _comboNumber = 0;
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

        public void DoLightAttack()
        {
            DoAttack(AttackType.Light);
        }

        public void DoStrongAttack()
        {
            DoAttack(AttackType.Strong);
        }

        public void PlayAnimation(AnimationClip animation)
        {
            _characterAnimations.PlayAnimationOverDefault(animation);
            //_animator.Play(animation);
        }

        public void PlayCharacterAnimations()
        {
            _characterAnimations.PlayDefaultAnimation();
            //_animator.Play(_characterAnimations);
        }

        public void DealLightCombo1Damage()
        {
            Debug.Log("light combo 1 damage");
        }

        public void DealLightCombo2Damage()
        {
            Debug.Log("light combo 2 damage");
        }

        public void DealLightCombo3Damage()
        {
            Debug.Log("light combo 3 damage");
        }

        void OnDrawGizmosSelected()
        {
            base.DrawGizmos();
        }

        private void DoAttack(AttackType attackType)
        {
            if(attackType == AttackType.None)
            {
                attackType = AttackType.None;
                _comboNumber = 0;
            }
            else if (_comboType == AttackType.Strong)
            {
                _comboNumber++;
                if (_comboNumber > MAX_COMBOS)
                {
                    _comboNumber = COMBO_START;
                }
            }
            else
            {
                _comboType = AttackType.Strong;
                _comboNumber = COMBO_START;
            }
            _attackAction = AttackType.None;
        }
    }
}
