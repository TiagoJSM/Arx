using GenericComponents.Interfaces.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericComponents.StateMachine
{
    public class StateContainer<TStateController, TAction>
    {
        private TStateController _controller;

        public IState<TStateController, TAction> State { get; private set; }
        public List<StateTransition<TStateController, TAction>> Transitions { get; private set; }
        public List<WhenTransitionToConfig<TStateController, TAction>> EventWhenTransitionTo { get; private set; }

        public StateContainer(IState<TStateController, TAction> state, TStateController controller)
        {
            _controller = controller;
            State = state;
            Transitions = new List<StateTransition<TStateController, TAction>>();
            EventWhenTransitionTo = new List<WhenTransitionToConfig<TStateController, TAction>>();
        }

        public StateTransition<TStateController, TAction> GetTransition(TAction action, float timeInState)
        {
            return Transitions.FirstOrDefault(t => t.Condition(_controller, action, /*State.TimeInState*/timeInState));
        }
    }
}
