using GenericComponents.Interfaces.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Enemies
{
    public interface ICharacter
    {
        bool Dead { get; }

        void DoMove(float move);
        void StayStill();
        void Die();
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

    public class MoveState : IState<ICharacter, StateAction>
    {
        public ICharacter StateController { get; set; }
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

    public class StandStillState : IState<ICharacter, StateAction>
    {
        public ICharacter StateController { get; set; }

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

    public class AttackState : IState<ICharacter, StateAction>
    {
        public ICharacter StateController { get; set; }

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

    public class DeathState : IState<ICharacter, StateAction>
    {
        public ICharacter StateController { get; set; }

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
}