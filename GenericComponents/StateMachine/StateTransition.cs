using Extensions;
using GenericComponents.Interfaces.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericComponents.StateMachine
{
    public class StateTransition<TStateController, TAction>
    {
        private Func<TStateController, TAction, float, bool> _condition;
        private StateContainer<TStateController, TAction> _stateContainer;

        public Func<TStateController, TAction, float, bool> Condition { get { return _condition; } }
        public IState<TStateController, TAction> State { get { return _stateContainer.State; } }
        public StateContainer<TStateController, TAction> StateContainer { get { return _stateContainer; } }

        public StateTransition(StateContainer<TStateController, TAction> stateContainer, Func<TStateController, TAction, float, bool> condition)
        {
            _stateContainer = stateContainer;
            _condition = condition;
        }
    }
}
