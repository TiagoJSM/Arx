using UnityEngine;
using System.Collections;
using GenericComponents.Controllers.Characters;
using System;
using System.Collections.Generic;
using Assets.Standard_Assets._2D.Scripts.Characters.Enemies;
using GenericComponents.Enums;
using CommonInterfaces.Controllers;
using Character = Assets.Standard_Assets._2D.Scripts.Characters.Enemies.ICharacter;
using Assets.Standard_Assets.Common;
using Extensions;
using Assets.Standard_Assets.Extensions;
using Assets.Standard_Assets.Common.Attributes;
using Assets.Standard_Assets.Weapons;
using Assets.Standard_Assets.Scripts.StateMachine;
using Assets.Standard_Assets._2D.Scripts.Controllers;

public class MeleeEnemyControllerStateManager : StateManager<Character, StateAction>
{
    public MeleeEnemyControllerStateManager(Character controller) : base(controller)
    {
        this.SetInitialState<StandStillState>()
            .To<GrappledState>((c, a, t) => c.Grappled)
            .To<TackingDamageState>((c, a, t) => c.InPain)
            .To<DeathState>((c, a, t) => c.Dead)
            .To<AttackState>((c, a, t) => a.Attack)
            .To<MoveState>((c, a, t) => a.Move != 0);

        this.From<AttackState>()
            .To<GrappledState>((c, a, t) => c.Grappled)
            .To<TackingDamageState>((c, a, t) => c.InPain)
            .To<DeathState>((c, a, t) => c.Dead)
            .To<StandStillState>((c, a, t) => a.Move == 0)
            .To<MoveState>((c, a, t) => a.Move != 0);

        this.From<MoveState>()
            .To<GrappledState>((c, a, t) => c.Grappled)
            .To<TackingDamageState>((c, a, t) => c.InPain)
            .To<DeathState>((c, a, t) => c.Dead)
            .To<AttackState>((c, a, t) => a.Attack)
            .To<StandStillState>((c, a, t) => a.Move == 0);

        this.From<TackingDamageState>()
            .To<GrappledState>((c, a, t) => c.Grappled)
            .To<StandStillState>((c, a, t) => c.InPainTime < t);

        this.From<GrappledState>()
            .To<StandStillState>((c, a, t) => !c.Grappled);
    }
}

[RequireComponent(typeof(CombatModule))]
[RequireComponent(typeof(GrappledCharacter))]
public class MeleeEnemyController : PlatformerCharacterController, Character
{
    private CombatModule _combatModule;
    private GrappledCharacter _grappled;
    private float _move;
    private bool _attack;
    private MeleeEnemyControllerStateManager _stateManager;
    private CommonVisualWeaponComponent _equippedWeapon;

    [SerializeField]
    private BaseCloseCombatWeapon _weaponPrefab;
    [SerializeField]
    public GameObject _weaponSocket;
    [SerializeField]
    private float _inPainTime = 0.4f;
    [SerializeField]
    private AudioSource[] _speakSounds;
    [SerializeField]
    private AudioSource _death;
    [SerializeField]
    [Layer]
    private int _sortingLayer;
    [SerializeField]
    private int _orderInLayer;

    public bool Attacking { get; private set; }
    public bool Dead { get; private set; }
    public float InPainTime { get { return _inPainTime; } }
    public GrappledCharacter GrappledCharacter { get { return _grappled; } }
    public bool Grappled { get { return _grappled.Grappled; } }

    public void Move(float move)
    {
        _move = move;
    }

    public void OrderAttack()
    {
        _speakSounds.PlayRandom();
        _combatModule.PrimaryAttack();
    }

    public override void Kill()
    {
        Dead = true;
        base.duckingCollider.enabled = false;
        base.standingCollider.enabled = false;
        ApplyMovementAndGravity = false;
        if (_death != null)
        {
            _death.Play();
        }
        ;
        var angle = Vector2.Angle(CharacterController2D.SlopeNormal, Vector2.up);
        var euler = transform.rotation.eulerAngles;
        SteadyRotation = false;
        transform.rotation = Quaternion.Euler(euler.x, euler.y, -angle);
    }

    public override int Attacked(GameObject attacker, int damage, Vector3? hitPoint, DamageType damageType, AttackTypeDetail attackType = AttackTypeDetail.Generic, int comboNumber = 1, bool showDamaged = false)
    {
        var damageTaken = base.Attacked(attacker, damage, hitPoint, damageType, attackType);
        if (_death != null)
        {
            _death.Play();
        }

        return damageTaken;
    }

    protected override void Awake()
    {
        base.Awake();
        _combatModule = GetComponent<CombatModule>();
        _grappled = GetComponent<GrappledCharacter>();
        _stateManager = new MeleeEnemyControllerStateManager(this);
        _combatModule.OnAttackStart += OnAttackStartHandler;
        _combatModule.OnEnterCombatState += OnEnterCombatStateHandler;
        _combatModule.OnCombatFinish += OnCombatFinishHandler;
        _grappled.OnGrappled += OnGrappledHandler;
    }

    protected override void Start()
    {
        base.Start();
        if(_weaponPrefab.RightHandWeapon != null)
        {
            _equippedWeapon = Instantiate(_weaponPrefab.RightHandWeapon);
            _equippedWeapon.transform.SetParent(_weaponSocket.transform, false);
            _equippedWeapon.SortingLayer = _sortingLayer;
            _equippedWeapon.OrderInLayer = _orderInLayer;
        }
        _combatModule.CloseCombatWeapon = _weaponPrefab;
    }

    protected override void Update()
    {
        _stateManager.Perform(new StateAction(_move, _attack));
        base.Update();
        _move = 0;
        _attack = false;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _combatModule.OnCombatFinish -= OnCombatFinishHandler;
    }

    private void OnAttackStartHandler(AttackType attackType, AttackStyle attackStyle, int combo)
    {
        StayStill();
    }

    private void OnEnterCombatStateHandler()
    {
        Attacking = true;
    }

    private void OnCombatFinishHandler()
    {
        Attacking = false;
    }

    private void OnGrappledHandler()
    {
        RaiseOnAttacked(_grappled.Grappler);
    }

    public void Die()
    {
    }

    public void ShowDamageTaken(bool taken)
    {
        //InPain = taken;
    }
}
