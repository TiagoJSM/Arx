using GenericComponents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericComponents.StateMachine
{
    public class StateManager<TStateContext, TAction> : IStateFactory<TStateContext, TAction>
    {
        private Dictionary<Type, IState<TStateContext, TAction>> _cachedStates;
        private IState<TStateContext, TAction> _currentState;

        public void Perform(TAction action)
        {
            var stateResult = _currentState.Perform(action);
            if(stateResult != _currentState)
            {
                _currentState.OnStateExit();
                stateResult.OnStateEnter();
                _currentState = stateResult;
            }
        }

        public TState State<TState>() where TState : class, IState<TStateContext, TAction>, new()
        {
            var type = typeof(TState);
            IState<TStateContext, TAction> cachedState;
            if (_cachedStates.TryGetValue(type, out cachedState))
            {
                return cachedState as TState;
            }
            var state = new TState();
            _cachedStates.Add(type, state);
            return state;
        }
    }
}
