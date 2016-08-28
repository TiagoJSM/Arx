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

public class FollowState : IState<ICharacterAI, object>
{
    public ICharacterAI StateController { get; set; }
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

public class IddleState : IState<ICharacterAI, object>
{
    public ICharacterAI StateController { get; set; }

    public float TimeInState { get; set; }

    public void OnStateEnter(object action)
    {
        StateController.StartIddle();
    }

    public void OnStateExit(object action)
    {
        StateController.StopIddle();
    }

    public void Perform(object action)
    {
    }
}

public class AttackTargetState : IState<ICharacterAI, object>
{
    public ICharacterAI StateController { get; set; }

    public float TimeInState { get; set; }

    public void OnStateEnter(object action)
    {
        StateController.StopMoving();
        StateController.Attack();
    }

    public void OnStateExit(object action)
    {
    }

    public void Perform(object action)
    {
    }
}