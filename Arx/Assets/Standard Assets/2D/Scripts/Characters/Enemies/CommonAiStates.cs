using UnityEngine;
using System.Collections;
using GenericComponents.Interfaces.States;
using System;

public interface ICharacterAI
{
    GameObject Target { get; }
    bool Attacking { get; }
    bool IsTargetInRange { get; }
    void MoveToTarget();
    void StopMoving();
    void StartIddle();
    void StopIddle();
    void Attack();
}

public abstract class BaseAiState<TAi> : IState<TAi, object> where TAi : ICharacterAI
{
    public TAi StateController { get; set; }
    public float TimeInState { get; set; }

    public virtual void OnStateEnter(object action)
    {
    }

    public virtual void OnStateExit(object action)
    {
    }

    public virtual void Perform(object action)
    {
    }
}

public class FollowState<TAi> : BaseAiState<TAi> where TAi : ICharacterAI
{
    public override void OnStateExit(object action)
    {
        StateController.StopMoving();
    }

    public override void Perform(object action)
    {
        StateController.MoveToTarget();
    }
}

public class IddleState<TAi> : BaseAiState<TAi> where TAi : ICharacterAI
{
    public override void OnStateEnter(object action)
    {
        StateController.StartIddle();
    }

    public override void OnStateExit(object action)
    {
        StateController.StopIddle();
    }
}

public class AttackTargetState<TAi> : BaseAiState<TAi> where TAi : ICharacterAI
{
    public override void OnStateEnter(object action)
    {
        StateController.StopMoving();
        StateController.Attack();
    }
}