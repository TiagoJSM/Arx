using CommonInterfaces.Controllers;
using Extensions;
using GenericComponents.Behaviours;
using GenericComponents.Controllers.Characters;
using GenericComponents.Enums;
using MathHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using CommonInterfaces.Enums;
using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine;
using Assets.Standard_Assets._2D.Scripts.Characters.Arx;
using Assets.Standard_Assets._2D.Scripts.EnvironmentDetection;
using Assets.Standard_Assets._2D.Scripts.Helpers;
using System.Collections;
using Assets.Standard_Assets._2D.Scripts.Controllers;
using Assets.Standard_Assets._2D.Scripts.Interaction;
using Assets.Standard_Assets.Common;
using Assets.Standard_Assets.Extensions;
using Assets.Standard_Assets.Weapons;
using Assets.Standard_Assets.Scripts.StateMachine;
using Assets.Standard_Assets._2D.Scripts.Footsteps;
using Assets.Standard_Assets._2D.Scripts.Combat;

[RequireComponent(typeof(CombatModule))]
[RequireComponent(typeof(LadderMovement))]
[RequireComponent(typeof(LadderFinder))]
[RequireComponent(typeof(MainCharacterNotification))]
[RequireComponent(typeof(CombatHitEffects))]
[RequireComponent(typeof(RopeMovement))]
[RequireComponent(typeof(MaterialFootstepPlayer))]
[RequireComponent(typeof(AimBehaviour))]
[RequireComponent(typeof(ThrowCombatBehaviour))]
public class MainPlatformerController : PlatformerCharacterController
{
    private CombatModule _combatModule;
    private LadderMovement _ladderMovement;
    private MainCharacterNotification _notifications;
    private CombatHitEffects _hitEffects;
    private RopeMovement _ropeMovement;
    private MaterialFootstepPlayer _footstepPlayer;
    private AimBehaviour _aimBehaviour;
    private StateManager<MainPlatformerController, PlatformerCharacterAction> _stateManager;

    private Rope _rope;
    private Coroutine _moveInParabolaCoroutine;
    private Pushable _pushable;
    private Vector3? _safeSpot;
    private LadderFinder _ladderFinder;
    private Coroutine _flashRoutine;
    private float _defaultMinYVelocity;
    private bool _canSlowGravityForAirAttack = true;
    private bool _changeVelocityMultiplierOnCombatFinish = true;

    [SerializeField]
    private float _rollingDuration = 1;
    [SerializeField]
    private float _maxRopeHorizontalForce = 20;
    [SerializeField]
    private float _ropeVerticalSpeed = 4;
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
    private AudioSource[] _attackShouts;
    [SerializeField]
    private AudioSource _rollSound;
    [SerializeField]
    private float _lightAirAttackGravitySlowDownTime = 4.0f;

    private float _move;
    private float _vertical;
    private bool _jump;
    private bool _roll;
    private bool _rollAfterAttack;
    private bool _releaseRope;
    private bool _aiming;
    private bool _shoot;
    private bool _throw;
    private bool _grabLadder;
    private bool _jumpOnLedge;

    private AttackType _attackAction;

    public event Action ShootAction;
    public event Action ThrowAction;

    public StateManager<MainPlatformerController, PlatformerCharacterAction> StateManager
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

    public ThrowCombatBehaviour ThrowCombatBehaviour { get; private set; }

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

    public float RopeClimbDirection { get { return _ropeMovement.RopeClimbDirection; } }

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

    public bool LadderFound { get { return _ladderFinder.LadderGameObject; } }

    public bool CollidesAbove { get { return CharacterController2D.collisionState.above; } }

    public bool TakingDamage { get; set; }

    public Vector3? HitPointThisFrame { get; private set; }

    public bool GrabbingLadder { get; private set; }
    public bool ShootWeaponEquipped { get; private set; }
    public bool ThrowWeaponEquipped { get; private set; }

    public void Move(float move, float vertical, bool jump, bool roll, bool releaseRope, bool aiming, bool jumpOnLedge)
    {
        _move = move;
        _vertical = vertical;
        _jump = jump;
        _releaseRope = releaseRope;
        _aiming = aiming;
        _jumpOnLedge = jumpOnLedge;

        if (Attacking)
        {
            _rollAfterAttack = roll | _rollAfterAttack;
        }
        else
        {
            _roll = roll;
        }
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
        _rollAfterAttack = false;
        base.Stand();
    }

    public void StartRoll()
    {
        _rollAfterAttack = false;
        _rollSound.Play();
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
        if(_ropeMovement.GrabRope(_rope))
        {
            ApplyMovementAndGravity = false;
            SteadyRotation = false;
        }
    }

    public void LetGoRope()
    {
        _ropeMovement.LetGoRope();
        _rope = null;
        ApplyMovementAndGravity = true;
        SteadyRotation = true;
    }

    public void MoveOnRope(float horizontal, float vertical)
    {
        _ropeMovement.MoveOnRope(horizontal, vertical);
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

    public void DoThrow()
    {
        ThrowCombatBehaviour.Throw(_aimBehaviour.GetWeaponAimAngle());
    }

    public void Aim(bool aim)
    {
        _aimBehaviour.enabled = aim;
    }

    public void Throw()
    {
        _throw = true;
    }

    //public void DoThrow()
    //{
    //    _combatModule.Throw();
    //    Attacking = true;
    //}

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
        ApplyMovementAndGravity = false;
        _ladderMovement.GrabLadder(_ladderFinder.LadderGameObject);
        GrabbingLadder = true;
    }

    public void MoveOnLadder(float vertical)
    {
        _ladderMovement.MoveOnLadder(vertical);
    }

    public void LetGoLadder()
    {
        ApplyMovementAndGravity = true;
        _ladderMovement.LetGoLadder();
        GrabbingLadder = false;
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
        this.Attacked(cause, damage, null, damageType);
    }

    public override int Attacked(
        GameObject attacker, 
        int damage, 
        Vector3? hitPoint,
        DamageType damageType,
        AttackTypeDetail attackType = AttackTypeDetail.Generic,
        int comboNumber = 1,
        bool showDamaged = false)
    {
        var damageTaken = base.Attacked(attacker, damage, hitPoint, damageType, attackType, comboNumber);
        if(damageTaken > 0)
        {
            _hitEffects.HitByEnemy();
            AttackedThisFrame = true;
            HitPointThisFrame = hitPoint;
            if (HitPointThisFrame == null)
            {
                HitPointThisFrame = attacker.transform.position;
            }
        }
        else if (showDamaged)
        {
            AttackedThisFrame = true;
            HitPointThisFrame = hitPoint;
            if (HitPointThisFrame == null)
            {
                HitPointThisFrame = attacker.transform.position;
            }
        }
        return damageTaken;
    }

    public void OnLanded()
    {
        _footstepPlayer.PlayLandingSound();
    }

    public void OnAirSlashLanded()
    {
        _slamAttackLand.Play();
    }

    public void StartLightAirAttack()
    {
        if (_canSlowGravityForAirAttack)
        {
            StartCoroutine(LightAirAttackGravitySlowDown());
            //VelocityMultiplier = new Vector2(VelocityMultiplier.x, 0.2f);
            _canSlowGravityForAirAttack = false;
        }
    }

    public void EndLightAirAttack()
    {
        //VelocityMultiplier = Vector2.one;
    }

    public void PerformShoot()
    {
        if(ShootAction != null)
        {
            ShootAction();
        }
    }

    public void PerformThrow()
    {
        if (ThrowAction != null)
        {
            ThrowAction();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _combatModule = GetComponent<CombatModule>();
        _ladderMovement = GetComponent<LadderMovement>();
        _ropeMovement = GetComponent<RopeMovement>();
        _ladderFinder = GetComponent<LadderFinder>();
        _notifications = GetComponent<MainCharacterNotification>();
        _hitEffects = GetComponent<CombatHitEffects>();
        _footstepPlayer = GetComponent<MaterialFootstepPlayer>();
        _aimBehaviour = GetComponent<AimBehaviour>();
        ThrowCombatBehaviour = GetComponent<ThrowCombatBehaviour>();
        _aimBehaviour.enabled = false;
        _stateManager = new PlatformerCharacterStateManager(this, _rollingDuration);
        _combatModule.OnEnterCombatState += OnEnterCombatStateHandler;
        _combatModule.OnAttackStart += OnAttackStartHandler;
        _combatModule.OnCombatFinish += OnCombatFinishHandler;
        CharacterController2D.onTriggerEnterEvent += OnTriggerEnterEventHandler;
        CharacterController2D.onTriggerExitEvent += OnTriggerExitEventHandler;
        _defaultMinYVelocity = CharacterController2D.MinYVelocity;

        ThrowWeaponEquipped = true;
    }

    protected override void Update()
    {
        base.Update();
        _pushable = FindPushables();
        _aimBehaviour.AimAngle = AimAngle;
        var action = 
            new PlatformerCharacterAction(
                _move, _vertical, _jump, _roll, _attackAction, 
                _releaseRope, _aiming, _shoot, _throw, _grabLadder, 
                _jumpOnLedge, _rollAfterAttack);
        _stateManager.Perform(action);
        _move = 0;
        _vertical = 0;
        _jump = false;
        _aiming = false;
        _shoot = false;
        _throw = false;
        AttackedThisFrame = false;
        HitPointThisFrame = null;
        _grabLadder = false;
        _jumpOnLedge = false;

        if (IsGrounded)
        {
            _canSlowGravityForAirAttack = true;
        }
        if (!Attacking)
        {
            _roll = false;
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
        if (_changeVelocityMultiplierOnCombatFinish)
        {
            VelocityMultiplier = Vector2.one;
        }
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

    public void StartFlashing()
    {
        if (_flashRoutine == null)
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

    private IEnumerator LightAirAttackGravitySlowDown()
    {
        _changeVelocityMultiplierOnCombatFinish = false;
        VelocityMultiplier = new Vector2(VelocityMultiplier.x, 0.35f);
        yield return new WaitForSeconds(_lightAirAttackGravitySlowDownTime);
        VelocityMultiplier = Vector2.one;
        _changeVelocityMultiplierOnCombatFinish = true;
    }
}
