using GenericComponents.Interfaces.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericComponents.StateMachine
{
    public class StateContainer<TStateController, TAction>
    {
        public IState<TStateController, TAction> State { get; private set; }
        public List<StateTransition<TStateController, TAction>> Transitions { get; private set; }

        public StateContainer(IState<TStateController, TAction> state)
        {
            State = state;
            Transitions = new List<StateTransition<TStateController, TAction>>();
        }
    }
}
