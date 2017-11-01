using UnityEngine;
using System.Collections;
using GenericComponents.Controllers.Characters;
using ArxGame.Components;
using GenericComponents.StateMachine;
using System;
using ArxGame.Components.Weapons;
using System.Collections.Generic;
using Assets.Standard_Assets._2D.Scripts.Characters.Enemies;
using GenericComponents.Enums;
using CommonInterfaces.Controllers;
using Character = Assets.Standard_Assets._2D.Scripts.Characters.Enemies.ICharacter;
using Assets.Standard_Assets.Common;

public class MeleeEnemyControllerStateManager : StateManager<Character, StateAction>
{
    public MeleeEnemyControllerStateManager(Character controller) : base(controller)
    {
        this.SetInitialState<StandStillState>()
            .To<TackingDamageState>((c, a, t) => c.HitLastTurn)
            .To<DeathState>((c, a, t) => c.Dead)
            .To<AttackState>((c, a, t) => a.Attack)
            .To<MoveState>((c, a, t) => a.Move != 0);

        this.From<AttackState>()
            .To<TackingDamageState>((c, a, t) => c.HitLastTurn)
            .To<DeathState>((c, a, t) => c.Dead)
            .To<StandStillState>((c, a, t) => a.Move == 0)
            .To<MoveState>((c, a, t) => a.Move != 0);

        this.From<MoveState>()
            .To<TackingDamageState>((c, a, t) => c.HitLastTurn)
            .To<DeathState>((c, a, t) => c.Dead)
            .To<AttackState>((c, a, t) => a.Attack)
            .To<StandStillState>((c, a, t) => a.Move == 0);

        this.From<TackingDamageState>()
            .To<StandStillState>((c, a, t) => c.InPainTime < t);
    }
}

[RequireComponent(typeof(CombatModule))]
public class MeleeEnemyController : PlatformerCharacterController, Character
{
    private CombatModule _combatModule;
    private float _move;
    private bool _attack;
    private MeleeEnemyControllerStateManager _stateManager;
    private GameObject _equippedWeapon;
    private float _lastHitDirection;

    [SerializeField]
    private BaseCloseCombatWeapon _weaponPrefab;
    [SerializeField]
    public GameObject _weaponSocket;
    [SerializeField]
    private float _inPainTime = 0.4f;

    public bool Attacking { get; private set; }
    public bool HitLastTurn { get; private set; }
    public bool Dead { get; private set; }
    public bool InPain { get; private set; }
    public float InPainTime { get { return _inPainTime; } }

    public void Move(float move)
    {
        _move = move;
    }

    public void OrderAttack()
    {
        _combatModule.PrimaryAttack();
    }

    public override void Kill()
    {
        Dead = true;
        base.duckingCollider.enabled = false;
        base.standingCollider.enabled = false;
        ApplyMovementAndGravity = false;
        StartCoroutine(CoroutineHelpers.DeathMovement(gameObject, _lastHitDirection, () => Destroy(gameObject)));
    }

    public override int Attacked(GameObject attacker, int damage, Vector3? hitPoint, DamageType damageType, AttackTypeDetail attackType = AttackTypeDetail.Generic, int comboNumber = 1)
    {
        var damageTaken = base.Attacked(attacker, damage, hitPoint, damageType, attackType);
        var damageOriginPosition = hitPoint ?? attacker.transform.position;
        _lastHitDirection = Math.Sign(transform.position.x - damageOriginPosition.x);
        HitLastTurn = true;
        return damage;
    }

    protected override void Awake()
    {
        base.Awake();
        _combatModule = GetComponent<CombatModule>();
        _stateManager = new MeleeEnemyControllerStateManager(this);
        _combatModule.OnAttackStart += OnAttackStartHandler;
        _combatModule.OnEnterCombatState += OnEnterCombatStateHandler;
        _combatModule.OnCombatFinish += OnCombatFinishHandler;
    }

    protected override void Start()
    {
        base.Start();
        _equippedWeapon = Instantiate(_weaponPrefab.RightHandWeapon.gameObject);
        _equippedWeapon.transform.SetParent(_weaponSocket.transform, false);
        _combatModule.CloseCombatWeapon = _weaponPrefab;
    }

    protected override void Update()
    {
        _stateManager.Perform(new StateAction(_move, _attack));
        base.Update();
        _move = 0;
        _attack = false;
        HitLastTurn = false;
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

    public void Die()
    {
    }

    public void ShowDamageTaken(bool taken)
    {
        InPain = taken;
    }
}
