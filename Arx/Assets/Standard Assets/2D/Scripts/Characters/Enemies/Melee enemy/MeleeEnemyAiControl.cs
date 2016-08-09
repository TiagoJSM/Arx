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
            .To<FollowState>((c, a, t) => !c.IsTargetInRange);
    }
}

[RequireComponent(typeof(MeleeEnemyController))]
public class MeleeEnemyAiControl : MonoBehaviour, ICharacterAI
{
    private Vector3 _startingPosition;
    private Coroutine _activeCoroutine;

    [SerializeField]
    private float _maxDistanceFromStartingPoint = 10;
    [SerializeField]
    private float _maxStoppedIddleTime = 5;
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
        _controller.Attack();
    }

    // Use this for initialization
    void Awake () {
        _controller = GetComponent<MeleeEnemyController>();
        _stateManager = new MeleeEnemyAiStateManager(this);
        _startingPosition = this.transform.position;
        _characterFinder.OnCharacterFound += OnCharacterFoundHandler;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        _stateManager.Perform(null);
    }

    void OnDestroy()
    {
        _characterFinder.OnCharacterFound -= OnCharacterFoundHandler;
    }

    private void IddleMovement()
    {
        StopActiveCoroutine();
        _activeCoroutine = StartCoroutine(IddleMovementCoroutine());
    }

    private void FollowTarget()
    {
        StopActiveCoroutine();
        _activeCoroutine = StartCoroutine(FollowTargetCoroutine());
    }

    private void StopActiveCoroutine()
    {
        if(_activeCoroutine != null)
        {
            StopCoroutine(_activeCoroutine);
        }
        _activeCoroutine = null;
    }

    private void OnCharacterFoundHandler(GenericComponents.Controllers.Characters.BasePlatformerController controller)
    {
        if(_target == null)
        {
            _target = controller.gameObject;
        }
    }

    private IEnumerator IddleMovementCoroutine()
    {
        var movementDirection = _controller.Direction;

        while (true)
        {
            _controller.Move(movementDirection.DirectionValue());
            yield return new WaitForFixedUpdate();
            var distance = Vector2.Distance(_startingPosition, this.transform.position);
            var directionOfStartingPoint = (this.transform.position.x - _startingPosition.x) >= 0 ? Direction.Right : Direction.Left;
            if (distance >= _maxDistanceFromStartingPoint && directionOfStartingPoint == movementDirection)
            {
                movementDirection = movementDirection == Direction.Left ? Direction.Right : Direction.Left;
                var stopTime = UnityEngine.Random.Range(0, _maxStoppedIddleTime);
                yield return new WaitForSeconds(stopTime);
                yield return new WaitForFixedUpdate();
            }
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
                //_target = null;
                yield break;
            }
            var currentPosition = this.transform.position;
            var xDifference = _target.transform.position.x - currentPosition.x;
            _controller.Move(xDifference);
            yield return new WaitForFixedUpdate();
        }
    }
}
