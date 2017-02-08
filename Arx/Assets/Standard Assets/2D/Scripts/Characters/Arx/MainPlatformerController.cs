﻿using CommonInterfaces.Controllers;
using CommonInterfaces.Weapons;
using Extensions;
using GenericComponents.Behaviours;
using GenericComponents.Controllers.Characters;
using GenericComponents.Enums;
using GenericComponents.StateMachine;
using MathHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using CommonInterfaces.Enums;
using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine;
using Assets.Standard_Assets._2D.Scripts.Characters.Arx;
using ArxGame.Components.Weapons;
using ArxGame.Components.Environment;
using Assets.Standard_Assets._2D.Scripts.EnvironmentDetection;
using Assets.Standard_Assets._2D.Scripts.Helpers;
using System.Collections;

[RequireComponent(typeof(CombatModule))]
public class MainPlatformerController : PlatformerCharacterController, IPlatformerCharacterController
{
    private CombatModule _combatModule;
    private StateManager<IPlatformerCharacterController, PlatformerCharacterAction> _stateManager;

    private Rope _rope;
    private RopePart _currentRopePart;
    private float _horizontalRopeMovement;
    private Coroutine _moveInParabolaCoroutine;
    private Pushable _pushable;
    private Vector3? _safeSpot;
    private Vector3? _hitPointThisFrame;

    [SerializeField]
    private float _rollingDuration = 1;
    [SerializeField]
    private float _maxRopeHorizontalForce = 20;
    [SerializeField]
    private float _ropeVerticalSpeed = 4;
    [SerializeField]
    private float _minimumDistanceFromRopeOrigin = 1;
    [SerializeField]
    private float _grappleRopeGrabHeightOffset = -6;
    [SerializeField]
    private float _objectPushForce = 1;
    [SerializeField]
    private Transform _pushableAreaP1;
    [SerializeField]
    private Transform _pushableAreaP2;

    private float _move;
    private float _vertical;
    private bool _jump;
    private bool _roll;
    private bool _releaseRope;
    private bool _aiming;
    private bool _shoot;
    private bool _throw;

    private AttackType _attackAction;

    public StateManager<IPlatformerCharacterController, PlatformerCharacterAction> StateManager
    {
        get
        {
            return _stateManager;
        }
    }

    public ICloseCombatWeapon CloseCombatWeapon
    {
        get
        {
            return _combatModule.CloseCombatWeapon;
        }
        set
        {
            _combatModule.CloseCombatWeapon = value;
        }
    }

    public IShooterWeapon ShooterWeapon
    {
        get
        {
            return _combatModule.ShooterWeapon;
        }
        set
        {
            _combatModule.ShooterWeapon = value;
        }
    }

    public ChainThrow ChainThrowWeapon
    {
        get
        {
            return _combatModule.ChainThrowWeapon;
        }
        set
        {
            _combatModule.ChainThrowWeapon = value;
        }
    }

    public bool IsAttackOver { get; private set; }

    public bool Attacking { get; private set; }

    public int ComboNumber
    {
        get
        {
            return _combatModule.ComboNumber;
        }
    }

    public WeaponType? WeaponType
    {
        get
        {
            return _combatModule.WeaponType;
        }
    }

    public float RollingDuration
    {
        get
        {
            return _rollingDuration;
        }
    }

    public bool IsCharging { get; private set; }

    public float AimAngle { get; set; }

    public bool RopeFound { get { return _rope != null; } }

    public float RopeClimbDirection { get; private set; }

    public GrappleRope GrappleRope { get { return _combatModule.GrappleRope; } }

    public Pushable Pushable
    {
        get
        {
            return _pushable;
        }
    }

    public Vector3? SafeSpot { get { return _safeSpot; } }

    public bool AttackedThisFrame { get; private set; }

    bool IPlatformerCharacterController.CanBeAttacked
    {
        get
        {
            return base.CanBeAttacked;
        }
        set
        {
            base.CanBeAttacked = value;
        }
    }

    public void Move(float move, float vertical, bool jump, bool roll, bool releaseRope, bool aiming)
    {
        _move = move;
        _vertical = vertical;
        _jump = jump;
        _roll = roll;
        _releaseRope = releaseRope;
        _aiming = aiming;
    }

    public void LightAttack()
    {
        _attackAction = AttackType.Primary;
    }

    public void StrongAttack()
    {
        _attackAction = AttackType.Secundary;
    }

    public void ChargeAttack()
    {
        IsCharging = true;
        _attackAction = AttackType.Primary;
    }

    public void ReleaseChargeAttack()
    {
        IsCharging = false;
        _attackAction = AttackType.None;
    }

    public void Shoot()
    {
        _shoot = true;
    }

    public void DoPrimaryGroundAttack()
    {
        VelocityMultiplier = new Vector2(0.4f, VelocityMultiplier.y);
        Attacking = _combatModule.PrimaryGroundAttack();
        IsAttackOver = !Attacking;
        _attackAction = AttackType.None;
    }

    public void DoSecundaryGroundAttack()
    {
        Attacking = _combatModule.SecundaryGroundAttack();
        IsAttackOver = !Attacking;
        _attackAction = AttackType.None;
    }

    public void DoPrimaryAirAttack()
    {
        Attacking = _combatModule.PrimaryAirAttack();
        IsAttackOver = !Attacking;
        _attackAction = AttackType.None;
    }

    public void DoSecundaryAirAttack()
    {
        Attacking = _combatModule.SecundaryAirAttack();
        IsAttackOver = !Attacking;
        _attackAction = AttackType.None;
    }

    public void DoChargeAttack()
    {
        Attacking = _combatModule.ChargeAttack();
        IsAttackOver = !Attacking;
        _attackAction = AttackType.None;
    }

    public void DoReleaseChargeAttack()
    {
        _combatModule.ReleaseChargeAttack();
    }

    public override void Duck()
    {
        _combatModule.ComboNumber = 0;
        Attacking = false;
        base.Duck();
    }

    public override void Stand()
    {
        _combatModule.ComboNumber = 0;
        Attacking = false;
        base.Stand();
    }

    public void StartIddle() { }
    public void StopIddle() { }

    public void FlipToSlideDownDirection()
    {
        if(CharacterController2D.SlopeNormal.x == 0)
        {
            return;
        }
        Flip(
            CharacterController2D.SlopeNormal.x > 0 
            ? CommonInterfaces.Enums.Direction.Right 
            : CommonInterfaces.Enums.Direction.Left);
    }

    public void AirSlash()
    {
        VelocityMultiplier = new Vector3(VelocityMultiplier.x, VelocityMultiplier.y * 4f);
        _combatModule.StartDiveAttack();
    }

    public void StopAirSlash()
    {
        VelocityMultiplier = Vector2.one;
        _combatModule.EndDiveAttack();
        OnAttackFinishHandler();
    }

    public void GrabRope()
    {
        if (_rope == null)
        {
            return;
        }

        ApplyMovementAndGravity = false;
        SteadyRotation = false;
        _currentRopePart = _rope.GetRopePartAt(this.transform.position);
        this.gameObject.transform.parent = _currentRopePart.transform;
    }

    public void LetGoRope()
    {
        this.gameObject.transform.parent = null;
        _currentRopePart = null;
        _rope = null;
        ApplyMovementAndGravity = true;
        SteadyRotation = true;
    }

    public void MoveOnRope(float horizontal, float vertical)
    {
        RopeClimbDirection = 0;

        var closestSegment = _rope.GetClosestRopeSegment(this.transform.position);
        this.gameObject.transform.parent = _currentRopePart.transform;
        this.gameObject.transform.position =
            FloatUtils.ClosestPointOnLine(closestSegment.Value.P1, closestSegment.Value.P2, this.transform.position);
        this.gameObject.transform.rotation = _currentRopePart.transform.rotation;

        if (Mathf.Abs(vertical) > 0.01)
        {
            var move = new Vector3(0, _ropeVerticalSpeed * Time.deltaTime * Mathf.Sign(vertical));
            var size = _rope.GetRopeSizeEndingIn(this.transform.position);
            var sizeAfterMove = size - move.y;

            if(CanClimpRope(sizeAfterMove, vertical))
            {
                this.transform.localPosition += move;
                _currentRopePart = _rope.GetRopePartAt(this.transform.position);
                this.gameObject.transform.parent = _currentRopePart.transform;
                RopeClimbDirection = vertical > 0 ? 1 : -1;
                return; //or we climb or we balance on the rope
            }
        }

        _horizontalRopeMovement = Mathf.Abs(horizontal) > 0.01f ? _maxRopeHorizontalForce * Math.Sign(horizontal) : 0;
    }

    public void DoAimingMove(float move)
    {
        base.DoMove(move, false);
    }

    public void SetDirectionToAimDirection()
    {
        if((AimAngle >= 0 && AimAngle <= 90) || (AimAngle <= 360 && AimAngle >= 270))
        {
            Flip(Direction.Right);
        }
        else
        {
            Flip(Direction.Left);
        }
    }

    public void DoShoot()
    {
        _combatModule.Shoot();
    }

    public void Aim(bool aim)
    {
        _combatModule.Aiming = aim;
    }

    public void Throw()
    {
        _throw = true;
    }

    public void DoThrow()
    {
        _combatModule.Throw();
        IsAttackOver = false;
    }

    public void GrabGrapple()
    {
        ApplyMovementAndGravity = false;
        SteadyRotation = false;
        DetectPlatform = false;
        // ChainThrowCombatBehaviour does the bellow line, 
        //but we still need to do it here, since it may be invalided by hit detection parenting
        this.transform.parent = _combatModule.GrappleRope.RopeEnd.gameObject.transform;
        this.transform.localPosition = new Vector3(0, _grappleRopeGrabHeightOffset);
    }

    public void MoveOnGrapple(float horizontal, float vertical)
    {
        if (Mathf.Abs(horizontal) > 0.01)
        {
            var body = _combatModule.GrappleRope.RopeEnd.GetComponent<Rigidbody2D>();
            body.AddForce(new Vector2(_maxRopeHorizontalForce * Math.Sign(horizontal), 0));
        }
        if (Mathf.Abs(vertical) > 0.01)
        {
            _combatModule.ClimbGrapple(vertical, _ropeVerticalSpeed);
            //var move = new Vector3(0, _ropeVerticalSpeed * Time.deltaTime * Mathf.Sign(vertical));
            //this.transform.localPosition += move;
        }
    }

    public void ReleaseGrapple()
    {
        _combatModule.ReleaseGrapple();
        this.transform.localRotation = Quaternion.identity;
        ApplyMovementAndGravity = true;
        SteadyRotation = true;
        DetectPlatform = true;
    }

    public void Push()
    {
        if(Mathf.Abs(HorizontalSpeed) < 0.01)
        {
            return;
        }
        var sign = Math.Sign(HorizontalSpeed);
        _pushable.Push(sign * _objectPushForce);
    }

    public void StartMovingToSafeSpot()
    {
        ApplyMovementAndGravity = false;
        var y = Mathf.Max(_safeSpot.Value.y, this.transform.position.y) + 2;
        var x = (_safeSpot.Value.x + this.transform.position.x) / 2;
        var distance = Vector2.Distance(this.transform.position, _safeSpot.Value);
        var distancePerSeconds = 40; // ToDo: move to inspector variable
        _moveInParabolaCoroutine = 
            StartCoroutine(
                MoveInParabola(
                    this.transform.position,
                    _safeSpot.Value,
                    new Vector2(x, y),
                    distance / distancePerSeconds,
                    ArrivedToSafeSpot));
    }

    public void StopMovingToSafeSpot()
    {
        ApplyMovementAndGravity = true;
        StopCoroutine(_moveInParabolaCoroutine);
        _moveInParabolaCoroutine = null;
    }

    public void Hit(GameObject cause, Vector3 safeSpot, int damage, DamageType damageType)
    {
        _safeSpot = safeSpot;
        base.Attacked(cause, damage, null, damageType);
    }

    public void LaunchCharacter(bool up = true)
    {
        if (up)
        {
            JumpUp();
        }
        float horizontalMovement = 0;
        if (_hitPointThisFrame != null)
        {
            horizontalMovement = Math.Sign(transform.position.x - _hitPointThisFrame.Value.x);
        }
        DoMove(horizontalMovement, false);
    }

    public void AttackStateDone()
    {
        VelocityMultiplier = Vector2.one;
    }

    public override float Attacked(
        GameObject attacker, 
        int damage, 
        Vector3? hitPoint,
        DamageType damageType,
        AttackTypeDetail attackType = AttackTypeDetail.Generic,
        int comboNumber = 1)
    {
        var damageTaken = base.Attacked(attacker, damage, hitPoint, damageType, attackType, comboNumber);
        if(damageTaken > 0)
        {
            AttackedThisFrame = true;
            _hitPointThisFrame = hitPoint;
            if (_hitPointThisFrame == null)
            {
                _hitPointThisFrame = attacker.transform.position;
            }
        }
        return damageTaken;
    }

    protected override void Awake()
    {
        base.Awake();
        _combatModule = GetComponent<CombatModule>();
        _stateManager = new PlatformerCharacterStateManager(this, _rollingDuration);
        _combatModule.OnAttackFinish += OnAttackFinishHandler;
        CharacterController2D.onTriggerEnterEvent += OnTriggerEnterEventHandler;
        CharacterController2D.onTriggerExitEvent += OnTriggerExitEventHandler;
    }

    protected override void Update()
    {
        base.Update();

        _pushable = FindPushables();
        _combatModule.AimAngle = AimAngle;
        var action = new PlatformerCharacterAction(_move, _vertical, _jump, _roll, _attackAction, _releaseRope, _aiming, _shoot, _throw);
        _stateManager.Perform(action);
        _move = 0;
        _vertical = 0;
        _jump = false;
        _roll = false;
        _aiming = false;
        _shoot = false;
        _throw = false;
        AttackedThisFrame = false;
        _hitPointThisFrame = null;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(_currentRopePart != null && _horizontalRopeMovement != 0)
        {
            _currentRopePart.PhysicsRopePart.AddForce(new Vector2(_horizontalRopeMovement, 0));
        }
    }

    private Pushable FindPushables()
    {
        if(_pushableAreaP1 == null || _pushableAreaP2 == null)
        {
            return null;
        }
        return 
            Physics2DHelpers
                .OverlapAreaAll<Pushable>(_pushableAreaP1.position, _pushableAreaP2.position)
                .FirstOrDefault();
    }

    private void OnAttackFinishHandler()
    {
        IsAttackOver = true;
    }

    private void OnTriggerEnterEventHandler(Collider2D collider)
    {
        if (!collider.IsTouching(CharacterController2D.BoxCollider2D))
        {
            return;
        }

        var rope = collider.gameObject.GetComponent<Rope>();
        if (rope == null)
        {
            return;
        }

        _rope = rope;
    }

    private void OnTriggerExitEventHandler(Collider2D collider)
    {
        if (_rope != null && _rope.gameObject == collider.gameObject)
        {
            _rope = null;
        }
    }

    private void ArrivedToSafeSpot()
    {
        _safeSpot = null;
    }

    private bool CanClimpRope(float ropeSizeAfterMove, float verticalMovement)
    {
        //if character is moving down movement is ok
        if(verticalMovement < 0)
        {
            return true;
        }
        //character can only move up if its at a distance from the rope top
        return _minimumDistanceFromRopeOrigin <= ropeSizeAfterMove;
    }
}
