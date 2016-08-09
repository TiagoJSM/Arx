using UnityEngine;
using System.Collections;
using GenericComponents.Controllers.Characters;
using ArxGame.Components;
using GenericComponents.StateMachine;
using System;
using ArxGame.Components.Weapons;

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
    private EnemyDetection _equippedWeapon;

    [SerializeField]
    private EnemyDetection _weaponPrefab;
    [SerializeField]
    public GameObject _weaponSocket;

    public bool Attacking
    {
        get
        {
            return _attacking;
        }
    }

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
        _combatModule.PrimaryAttack();
        _attacking = true;
    }

    protected override void Awake()
    {
        base.Awake();
        _combatModule = GetComponent<CombatModule>();
        _stateManager = new MeleeEnemyControllerStateManager(this);
        _combatModule.OnAttackFinish += OnAttackFinishHandler;
    }

    protected virtual void Start()
    {
        _equippedWeapon = Instantiate(_weaponPrefab);
        _equippedWeapon.transform.SetParent(_weaponSocket.transform, false);
        _combatModule.Weapon = _equippedWeapon;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        _stateManager.Perform(new StateAction(_move, _attack));
        _move = 0;
        _attack = false;
    }

    void OnDestroy()
    {
        _combatModule.OnAttackFinish += OnAttackFinishHandler;
    }

    private void OnAttackFinishHandler()
    {
        _attacking = false;
    }
}
