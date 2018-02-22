using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.Scripts.StateMachine
{
    public class TransitionConfiguration<TStateController, TAction>
    {
        private IStateFactory<TStateController, TAction> _factory;
        private List<StateContainer<TStateController, TAction>> _states;

        public TransitionConfiguration(
            IStateFactory<TStateController, TAction> factory,
            StateContainer<TStateController, TAction> state)
        {
            _factory = factory;
            _states = new List<StateContainer<TStateController, TAction>>() { state };
        }

        public TransitionConfiguration<TStateController, TAction> To<TState>(
            Func<TStateController, TAction, float, bool> condition) where TState : class, IState<TStateController, TAction>, new()
        {
            var stateContainer = _factory.GetContainer<TState>();
            for(var idx = 0; idx < _states.Count; idx++)
            {
                _states[idx].Transitions.Add(new StateTransition<TStateController, TAction>(stateContainer, condition));
            }
            
            return this;
        }

        public TransitionConfiguration<TStateController, TAction> WhenTransitionTo<TState>(
            Action<TStateController, TAction> action) where TState : class, IState<TStateController, TAction>, new()
        {
            var stateContainer = _factory.GetContainer<TState>();
            for (var idx = 0; idx < _states.Count; idx++)
            {
                _states[idx].EventWhenTransitionTo.Add(new WhenTransitionToConfig<TStateController, TAction>(typeof(TState), action));
            }
            return this;
        }

        public TransitionConfiguration<TStateController, TAction> And<TState>() where TState : class, IState<TStateController, TAction>, new()
        {
            var container = _factory.GetContainer<TState>();
            _states.Add(container);
            return this;
        }
    }
}
