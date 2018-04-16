using Assets.Standard_Assets.Scripts.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Enemies
{
    public interface ICharacterController
    {
        bool Dead { get; }
        bool HitLastTurn { get; }
        float LastHitDirection { get; }
        float InPainTime { get; }
        bool InPain { get; set; }
        bool Grappled { get; }

        void DoMove(float move);
        void DoMove(float move, bool setDirectionToMovement);
        void DoMove(float move, float speed, bool setDirectionToMovement);
        void StayStill();
        void Die();
        void Push(Vector2 force);
    }

    public class StateAction
    {
        public float Move { get; private set; }
        public bool Attack { get; private set; }

        public StateAction(float move, bool attack)
        {
            Move = move;
            Attack = attack;
        }
    }

    public class MoveState : IState<ICharacterController, StateAction>
    {
        public ICharacterController StateController { get; set; }
        public float TimeInState { get; set; }

        public void OnStateEnter(StateAction action)
        {
        }

        public void OnStateExit(StateAction action)
        {
        }

        public void Perform(StateAction action)
        {
            StateController.DoMove(action.Move);
        }
    }

    public class StandStillState : IState<ICharacterController, StateAction>
    {
        public ICharacterController StateController { get; set; }

        public float TimeInState { get; set; }

        public void OnStateEnter(StateAction action)
        {
            StateController.StayStill();
        }

        public void Perform(StateAction action)
        {
        }
        public void OnStateExit(StateAction action)
        {
        }
    }

    public class AttackState : IState<ICharacterController, StateAction>
    {
        public ICharacterController StateController { get; set; }

        public float TimeInState { get; set; }

        public void OnStateEnter(StateAction action)
        {
        }

        public void Perform(StateAction action)
        {
        }
        public void OnStateExit(StateAction action)
        {
        }
    }

    public class DeathState : IState<ICharacterController, StateAction>
    {
        public ICharacterController StateController { get; set; }

        public float TimeInState { get; set; }

        public void OnStateEnter(StateAction action)
        {
            StateController.StayStill();
            StateController.Die();
        }

        public void OnStateExit(StateAction action)
        {
        }

        public void Perform(StateAction action)
        {
        }
    }

    public class TackingDamageState : IState<ICharacterController, StateAction>
    {
        public ICharacterController StateController { get; set; }

        public float TimeInState { get; set; }

        public void OnStateEnter(StateAction action)
        {
        }

        public void OnStateExit(StateAction action)
        {
            StateController.InPain = false;
        }

        public void Perform(StateAction action)
        {
            StateController.DoMove(-StateController.LastHitDirection, false);
        }
    }

    public class GrappledState : IState<ICharacterController, StateAction>
    {
        public ICharacterController StateController { get; set; }

        public float TimeInState { get; set; }

        public void OnStateEnter(StateAction action)
        {
            StateController.StayStill();
        }

        public void OnStateExit(StateAction action)
        {
        }

        public void Perform(StateAction action)
        {
        }
    }
}