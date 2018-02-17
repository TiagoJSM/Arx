using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.Scripts.StateMachine
{
    public class TransitionConfiguration<TStateController, TAction>
    {
        private IStateFactory<TStateController, TAction> _factory;
        private StateContainer<TStateController, TAction> _state;

        public TransitionConfiguration(
            IStateFactory<TStateController, TAction> factory,
            StateContainer<TStateController, TAction> state)
        {
            _factory = factory;
            _state = state;
        }

        public TransitionConfiguration<TStateController, TAction> To<TState>(
            Func<TStateController, TAction, float, bool> condition) where TState : class, IState<TStateController, TAction>, new()
        {
            var stateContainer = _factory.GetContainer<TState>();
            _state.Transitions.Add(new StateTransition<TStateController, TAction>(stateContainer, condition));
            return this;
        }

        public TransitionConfiguration<TStateController, TAction> WhenTransitionTo<TState>(
            Action<TStateController, TAction> action) where TState : class, IState<TStateController, TAction>, new()
        {
            var stateContainer = _factory.GetContainer<TState>();
            _state.EventWhenTransitionTo.Add(new WhenTransitionToConfig<TStateController, TAction>(typeof(TState), action));
            return this;
        }
    }
}
