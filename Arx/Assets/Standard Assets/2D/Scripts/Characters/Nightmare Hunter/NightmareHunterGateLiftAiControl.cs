using UnityEngine;
using System.Collections;
using ArxGame.Components.AnimationControllers;
using CommonInterfaces.Controls;
using GenericComponents.Interfaces.States;
using GenericComponents.StateMachine;

public interface INightmareHunterGateLiftAiControl
{
    bool ReachedGate { get; }
    bool ReachedTarget { get; }

    void MoveToTarget();
    void StopMoving();
    void KnockGate();
    void AttackTarget();
    void MoveAway();
    void Die();
}

public class NightmareHunterGateAiStateManager : StateManager<INightmareHunterGateLiftAiControl, object>
{
    private class FollowState : IState<INightmareHunterGateLiftAiControl, object>
    {
        public INightmareHunterGateLiftAiControl StateController { get; set; }
        public float TimeInState { get; set; }

        public void OnStateEnter(object action)
        {

        }

        public void OnStateExit(object action)
        {
            StateController.StopMoving();
        }

        public void Perform(object action)
        {
            StateController.MoveToTarget();
        }
    }

    private class ReachedGateState : IState<INightmareHunterGateLiftAiControl, object>
    {
        public INightmareHunterGateLiftAiControl StateController { get; set; }
        public float TimeInState { get; set; }

        public void OnStateEnter(object action)
        {
            StateController.KnockGate();
        }

        public void OnStateExit(object action)
        {
        }

        public void Perform(object action)
        {
        }
    }

    private class ReachedTargetState : IState<INightmareHunterGateLiftAiControl, object>
    {
        public INightmareHunterGateLiftAiControl StateController { get; set; }
        public float TimeInState { get; set; }

        public void OnStateEnter(object action)
        {
            StateController.AttackTarget();
        }

        public void OnStateExit(object action)
        {
        }

        public void Perform(object action)
        {
        }
    }

    private class MoveAwayState : IState<INightmareHunterGateLiftAiControl, object>
    {
        public INightmareHunterGateLiftAiControl StateController { get; set; }
        public float TimeInState { get; set; }

        public void OnStateEnter(object action)
        {
            StateController.MoveAway();
        }

        public void OnStateExit(object action)
        {
        }

        public void Perform(object action)
        {
        }
    }

    private class DeathState : IState<INightmareHunterGateLiftAiControl, object>
    {
        public INightmareHunterGateLiftAiControl StateController { get; set; }
        public float TimeInState { get; set; }

        public void OnStateEnter(object action)
        {
            StateController.Die();
        }

        public void OnStateExit(object action)
        {
        }

        public void Perform(object action)
        {
        }
    }

    public NightmareHunterGateAiStateManager(INightmareHunterGateLiftAiControl controller)
        : base(controller)
    {
        this
            .SetInitialState<FollowState>()
                .To<ReachedGateState>((c, a, t) => c.ReachedGate)
                .To<ReachedTargetState>((c, a, t) => c.ReachedTarget);

        this
            .From<ReachedGateState>()
                .To<MoveAwayState>((c, a, t) => t > 4);

        this
            .From<MoveAwayState>()
                .To<DeathState>((c, a, t) => t > 4);
    }
}

[RequireComponent(typeof(NightmareHunterController))]
public class NightmareHunterGateLiftAiControl : MonoBehaviour, IPlatformerAICharacterControl, INightmareHunterGateLiftAiControl
{
    private NightmareHunterGateAiStateManager _aiManager;
    private NightmareHunterController _characterController;

    public GameObject moveAwayObject;
    public GameObject gate;
    public GameObject target;
    public float targetTreshold = 1;
    public AnimationClip knockGateAnimation;

    private Vector2? _moveToPosition;
    private float _treshold;

    protected NightmareHunterController CharacterController
    {
        get
        {
            return _characterController;
        }
    }

    public bool ReachedGate
    {
        get; private set;
    }

    public bool ReachedTarget
    {
        get; private set;
    }

    public void MoveToTarget()
    {
        MoveDirectlyTo(target.transform.position.ToVector2(), targetTreshold);
    }

    public void KnockGate()
    {
    }

    public void AttackTarget()
    {
    }

    public void MoveAway()
    {
        MoveDirectlyTo(moveAwayObject.transform.position.ToVector2(), targetTreshold);
    }

    public void Die()
    {
    }

    public void MoveDirectlyTo(Vector2 position, float treshold)
    {
        _moveToPosition = position;
        _treshold = treshold;
    }

    public void StopMoving()
    {
        _moveToPosition = null;
    }

    protected void Start()
    {
        _characterController = GetComponent<NightmareHunterController>();
        _aiManager = new NightmareHunterGateAiStateManager(this);
    }

    protected void FixedUpdate()
    {
        _aiManager.Perform(null);
        if (_moveToPosition == null)
        {
            return;
        }

        var currentPosition = this.transform.position.ToVector2();
        var distance = Vector2.Distance(currentPosition, _moveToPosition.Value);
        if (distance < _treshold)
        {
            _moveToPosition = null;
            return;
        }
        var xDifference = _moveToPosition.Value.x - currentPosition.x;
        _characterController.Move(xDifference);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == gate)
        {
            ReachedGate = true;
        }
        if (other.gameObject == target)
        {
            ReachedTarget = true;
        }
    }
}
