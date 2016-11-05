using UnityEngine;
using System.Collections;
using GenericComponents.Controllers.Characters;
using ArxGame.Components;
using GenericComponents.StateMachine;
using System;
using ArxGame.Components.Weapons;
using CommonInterfaces.Controllers;
using System.Collections.Generic;
using ArxGame.Components.Combat;

public class MeleeEnemyControllerStateManager : StateManager<ICharacter, StateAction>
{
    public MeleeEnemyControllerStateManager(ICharacter controller) : base(controller)
    {
        this.SetInitialState<StandStillState>()
            .To<AttackState>((c, a, t) => a.Attack)
            .To<MoveState>((c, a, t) => a.Move != 0);

        this.From<AttackState>()
            .To<StandStillState>((c, a, t) => a.Move == 0)
            .To<MoveState>((c, a, t) => a.Move != 0);

        this.From<MoveState>()
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
    private BaseCloseCombatWeapon _equippedWeapon;

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
        _equippedWeapon = Instantiate(_weaponPrefab);
        _equippedWeapon.transform.SetParent(_weaponSocket.transform, false);
        _combatModule.Weapon = _equippedWeapon;
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
