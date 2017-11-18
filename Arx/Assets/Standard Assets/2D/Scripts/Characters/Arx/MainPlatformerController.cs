using CommonInterfaces.Controllers;
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
using Assets.Standard_Assets._2D.Scripts.Controllers;
using Assets.Standard_Assets._2D.Scripts.Interaction;
using Assets.Standard_Assets.Common;
using Assets.Standard_Assets.Extensions;

[RequireComponent(typeof(CombatModule))]
[RequireComponent(typeof(LadderMovement))]
[RequireComponent(typeof(LadderFinder))]
[RequireComponent(typeof(MainCharacterNotification))]
public class MainPlatformerController : PlatformerCharacterController, IPlatformerCharacterController
{
    private CombatModule _combatModule;
    private LadderMovement _ladderMovement;
    private MainCharacterNotification _notifications;
    private StateManager<IPlatformerCharacterController, PlatformerCharacterAction> _stateManager;

    private Rope _rope;
    private RopePart _currentRopePart;
    private float _horizontalRopeMovement;
    private Coroutine _moveInParabolaCoroutine;
    private Pushable _pushable;
    private Vector3? _safeSpot;
    private Vector3? _hitPointThisFrame;
    private LadderFinder _ladderFinder;
    private Coroutine _flashRoutine;
    private float _defaultMinYVelocity;
    private bool _canSlowGravityForAirAttack = true;

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
    [SerializeField]
    private float _groundAttackVelocity = 0.75f;
    [SerializeField]
    private GameObject[] _flashingObjects;
    [SerializeField]
    private AudioSource _slamAttackAir;
    [SerializeField]
    private AudioSource _slamAttackLand;
    [SerializeField]
    private AudioSource _landed;
    [SerializeField]
    private AudioSource[] _attackShouts;

    private float _move;
    private float _vertical;
    private bool _jump;
    private bool _roll;
    private bool _releaseRope;
    private bool _aiming;
    private bool _shoot;
    private bool _throw;
    private bool _grabLadder;

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

    public bool LadderFound { get { return _ladderFinder.LadderGameObject; } }

    public void Move(float move, float vertical, bool jump, bool roll, bool releaseRope, bool aiming)
    {
        _move = move;
        _vertical = vertical;
        _jump = jump;
        _roll = roll;
        _releaseRope = releaseRope;
        _aiming = aiming;
    }

    public void RequestGrabLadder()
    {
        _grabLadder = true;
    }

    public void LightAttack()
    {
        _combatModule.PrimaryAttack();
        _attackAction = AttackType.Primary;
    }

    public void StrongAttack()
    {
        _combatModule.SecundaryAttack();
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

    public void DoChargeAttack()
    {
        Attacking = _combatModule.ChargeAttack();
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
        if(Velocity.x == 0)
        {
            return;
        }
        Flip(
            Velocity.x > 0 
            ? CommonInterfaces.Enums.Direction.Right 
            : CommonInterfaces.Enums.Direction.Left);
    }

    public void AirSlash()
    {
        CharacterController2D.MinYVelocity = 2 * _defaultMinYVelocity;
        VelocityMultiplier = new Vector3(VelocityMultiplier.x, VelocityMultiplier.y * 4f);
        _combatModule.StartDiveAttack();
        _slamAttackAir.Play();
    }

    public void StopAirSlash()
    {
        CharacterController2D.MinYVelocity = _defaultMinYVelocity;
        VelocityMultiplier = Vector2.one;
        _attackAction = AttackType.None;
        Attacking = false;
        _combatModule.EndDiveAttack();
        OnCombatFinishHandler();
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
        Attacking = true;
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

    public void PushObject()
    {
        if(Mathf.Abs(HorizontalSpeed) < 0.01)
        {
            return;
        }
        var sign = Math.Sign(HorizontalSpeed);
        _pushable.Push(sign * _objectPushForce);
    }

    public void GrabLadder()
    {
        _notifications.HideInteraction();
        _ladderMovement.GrabLadder();
    }

    public void MoveOnLadder(float vertical)
    {
        _ladderMovement.MoveOnLadder(vertical);
    }

    public void LetGoLadder()
    {
        _ladderMovement.LetGoLadder();
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

    public void LaunchCharacter()
    {
        float horizontalMovement = 0;
        if (_hitPointThisFrame != null)
        {
            horizontalMovement = Math.Sign(transform.position.x - _hitPointThisFrame.Value.x);
        }
        DesiredMovementVelocity = Vector2.zero;
        //ToDo: move this into globals?
        Push(new Vector2(20 * horizontalMovement, 600));
    }

    public void AttackStateDone()
    {
        VelocityMultiplier = Vector2.one;
    }

    public override int Attacked(
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

    public void OnLanded()
    {
        _landed.Play();
    }

    public void OnAirSlashLanded()
    {
        _slamAttackLand.Play();
    }

    public void StartLightAirAttack()
    {
        if (_canSlowGravityForAirAttack)
        {
            VelocityMultiplier = new Vector2(VelocityMultiplier.x, 0.2f);
            _canSlowGravityForAirAttack = false;
        }
    }

    public void EndLightAirAttack()
    {
        VelocityMultiplier = Vector2.one;
    }

    protected override void Awake()
    {
        base.Awake();
        _combatModule = GetComponent<CombatModule>();
        _ladderMovement = GetComponent<LadderMovement>();
        _ladderFinder = GetComponent<LadderFinder>();
        _notifications = GetComponent<MainCharacterNotification>();
        _stateManager = new PlatformerCharacterStateManager(this, _rollingDuration);
        _combatModule.OnEnterCombatState += OnEnterCombatStateHandler;
        _combatModule.OnAttackStart += OnAttackStartHandler;
        _combatModule.OnCombatFinish += OnCombatFinishHandler;
        CharacterController2D.onTriggerEnterEvent += OnTriggerEnterEventHandler;
        CharacterController2D.onTriggerExitEvent += OnTriggerExitEventHandler;
        _defaultMinYVelocity = CharacterController2D.MinYVelocity;
    }

    protected override void Update()
    {
        base.Update();

        _pushable = FindPushables();
        _combatModule.AimAngle = AimAngle;
        var action = 
            new PlatformerCharacterAction(
                _move, _vertical, _jump, _roll, _attackAction, 
                _releaseRope, _aiming, _shoot, _throw, _grabLadder);
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
        _grabLadder = false;

        if (IsGrounded)
        {
            _canSlowGravityForAirAttack = true;
        }
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

    private void OnEnterCombatStateHandler()
    {
        Attacking = true;
    }

    private void OnAttackStartHandler(AttackType attackType, AttackStyle attackStyle, int combo)
    {
        _attackShouts.PlayRandom();
        if (attackStyle == AttackStyle.Ground)
        {
            var x = attackType == AttackType.Primary ? _groundAttackVelocity : 0;
            VelocityMultiplier = new Vector2(x, VelocityMultiplier.y);
        }
    }

    private void OnCombatFinishHandler()
    {
        VelocityMultiplier = Vector2.one;
        _attackAction = AttackType.None;
        Attacking = false;
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

    public void StartFlashing()
    {
        if(_flashRoutine == null)
        {
            _flashRoutine = StartCoroutine(CoroutineHelpers.Flash(() => StopFlashing(), _flashingObjects));
        }
    }

    public void StopFlashing()
    {
        CanBeAttacked = true;
        StopCoroutine(_flashRoutine);
        _flashRoutine = null;
        for (var idx = 0; idx < _flashingObjects.Length; idx++)
        {
            _flashingObjects[idx].SetActive(true);
        }
    }
}
