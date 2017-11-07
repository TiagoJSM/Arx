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
        this.SetInitialState<IddleState<ICharacterAI>>()
            .To<FollowState<ICharacterAI>>((c, o, t) => c.Target != null);

        this.From<FollowState<ICharacterAI>>()
            .To<AttackTargetState<ICharacterAI>>((c, a, t) => c.IsTargetInRange)
            .To<IddleState<ICharacterAI>>((c, o, t) => c.Target == null);

        this.From<AttackTargetState<ICharacterAI>>()
            .To<FollowState<ICharacterAI>>((c, a, t) => !c.IsTargetInRange && !c.Attacking);
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

    protected override Vector2 Velocity
    {
        get
        {
            return _controller.Velocity;
        }
    }

    public void MoveToTarget()
    {
        _controller.MovementType = MovementType.Run;
        FollowTarget();
    }

    public void StartIddle()
    {
        _controller.MovementType = MovementType.Walk;
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

    public void OrderAttack()
    {
        StopActiveCoroutine();
        _controller.OrderAttack();
    }

    // Use this for initialization
    protected override void Awake() {
        base.Awake();
        _controller = GetComponent<MeleeEnemyController>();
        _stateManager = new MeleeEnemyAiStateManager(this);
        _characterFinder.OnCharacterFound += OnCharacterFoundHandler;
    }

    public override void Move(float directionValue)
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
        _controller.MovementType = MovementType.Run;
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
