using UnityEngine;
using System.Collections;
using System;
using GenericComponents.StateMachine;
using Extensions;
using Assets.Standard_Assets._2D.Scripts.Characters;
using CommonInterfaces.Enums;

public class MeleeEnemyAiStateManager : StateManager<ICharacterAI, object>
{
    public MeleeEnemyAiStateManager(ICharacterAI controller) : base(controller)
    {
        this.SetInitialState<IddleState>()
            .To<FollowState>((c, o, t) => c.Target != null);

        this.From<FollowState>()
            .To<AttackTargetState>((c, a, t) => c.IsTargetInRange)
            .To<IddleState>((c, o, t) => c.Target == null);

        this.From<AttackTargetState>()
            .To<FollowState>((c, a, t) => !c.IsTargetInRange && !c.Attacking);
    }
}

[RequireComponent(typeof(MeleeEnemyController))]
public class MeleeEnemyAiControl : PlatformerCharacterAiControl, ICharacterAI
{
    [SerializeField]
    private CharacterFinder _characterFinder;
    [SerializeField]
    private float _attackRange = 1;

    private GameObject _target;
    private MeleeEnemyController _controller;
    private MeleeEnemyAiStateManager _stateManager;

    public GameObject Target
    {
        get
        {
            return _target;
        }
    }

    public bool Attacking
    {
        get
        {
            return _controller.Attacking;
        }
    }

    public bool IsTargetInRange
    {
        get
        {
            var currentPosition = this.transform.position;
            var distance = Vector2.Distance(currentPosition, _target.transform.position);
            return distance < _attackRange;
        }
    }

    protected override Direction CurrentDirection
    {
        get
        {
            return _controller.Direction;
        }
    }

    public void MoveToTarget()
    {
        FollowTarget();
    }

    public void StartIddle()
    {
        IddleMovement();
    }

    public void StopIddle()
    {
        StopActiveCoroutine();
    }

    public void StopMoving()
    {
        StopActiveCoroutine();
    }

    public void Attack()
    {
        StopActiveCoroutine();
        _controller.StayStill();
        _controller.Attack();
    }

    // Use this for initialization
    protected override void Awake() {
        base.Awake();
        _controller = GetComponent<MeleeEnemyController>();
        _stateManager = new MeleeEnemyAiStateManager(this);
        _characterFinder.OnCharacterFound += OnCharacterFoundHandler;
    }

    protected override void Move(float directionValue)
    {
        _controller.Move(directionValue);
    }

    void Update()
    {
        _stateManager.Perform(null);
    }

    void OnDestroy()
    {
        _characterFinder.OnCharacterFound -= OnCharacterFoundHandler;
    }

    private void FollowTarget()
    {
        SetActiveCoroutine(FollowTargetCoroutine());
    }

    private void OnCharacterFoundHandler(GenericComponents.Controllers.Characters.BasePlatformerController controller)
    {
        if(_target == null)
        {
            _target = controller.gameObject;
        }
    }

    private IEnumerator FollowTargetCoroutine()
    {
        if (_target == null)
        {
            yield break;
        }

        while (true)
        {
            if (IsTargetInRange)
            {
                yield break;
            }
            var currentPosition = this.transform.position;
            var xDifference = _target.transform.position.x - currentPosition.x;
            _controller.Move(xDifference);
            yield return null;
        }
    }
}
