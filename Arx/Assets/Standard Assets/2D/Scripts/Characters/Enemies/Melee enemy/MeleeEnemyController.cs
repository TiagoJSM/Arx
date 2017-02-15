using UnityEngine;
using System.Collections;
using GenericComponents.Controllers.Characters;
using ArxGame.Components;
using GenericComponents.StateMachine;
using System;
using ArxGame.Components.Weapons;
using System.Collections.Generic;
using Assets.Standard_Assets._2D.Scripts.Characters.Enemies;

public class MeleeEnemyControllerStateManager : StateManager<ICharacter, StateAction>
{
    public MeleeEnemyControllerStateManager(ICharacter controller) : base(controller)
    {
        this.SetInitialState<StandStillState>()
            .To<DeathState>((c, a, t) => c.Dead)
            .To<AttackState>((c, a, t) => a.Attack)
            .To<MoveState>((c, a, t) => a.Move != 0);

        this.From<AttackState>()
            .To<DeathState>((c, a, t) => c.Dead)
            .To<StandStillState>((c, a, t) => a.Move == 0)
            .To<MoveState>((c, a, t) => a.Move != 0);

        this.From<MoveState>()
            .To<DeathState>((c, a, t) => c.Dead)
            .To<AttackState>((c, a, t) => a.Attack)
            .To<StandStillState>((c, a, t) => a.Move == 0);
    }
}

[RequireComponent(typeof(CombatModule))]
public class MeleeEnemyController : PlatformerCharacterController, ICharacter
{
    private bool _attacking;
    private CombatModule _combatModule;
    private float _move;
    private bool _attack;
    private MeleeEnemyControllerStateManager _stateManager;
    private GameObject _equippedWeapon;

    [SerializeField]
    private BaseCloseCombatWeapon _weaponPrefab;
    [SerializeField]
    public GameObject _weaponSocket;

    public bool Attacking
    {
        get
        {
            return _attacking;
        }
    }

    public bool Dead { get; private set; }

    public void Move(float move)
    {
        _move = move;
    }

    public void Attack()
    {
        _attack = true;
    }

    public void DoAttack()
    {
        _combatModule.PrimaryGroundAttack();
        _attacking = true;
    }

    public override void Kill()
    {
        Dead = true;
    }

    protected override void Awake()
    {
        base.Awake();
        _combatModule = GetComponent<CombatModule>();
        _stateManager = new MeleeEnemyControllerStateManager(this);
        _combatModule.OnAttackFinish += OnAttackFinishHandler;
    }

    protected override void Start()
    {
        base.Start();
        _equippedWeapon = Instantiate(_weaponPrefab.RightHandWeapon);
        _equippedWeapon.transform.SetParent(_weaponSocket.transform, false);
        _combatModule.CloseCombatWeapon = _weaponPrefab;
    }

    protected override void Update()
    {
        base.Update();
        _stateManager.Perform(new StateAction(_move, _attack));
        _move = 0;
        _attack = false;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _combatModule.OnAttackFinish += OnAttackFinishHandler;
    }

    private void OnAttackFinishHandler()
    {
        _attacking = false;
    }
}
