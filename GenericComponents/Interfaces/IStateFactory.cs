using GenericComponents.Interfaces.States;
using GenericComponents.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericComponents.Interfaces
{
    public interface IStateFactory<TStateContext, TAction>
    {
        TState State<TState>() where TState : class, IState<TStateContext, TAction>, new();
        StateContainer<TStateContext, TAction> GetContainer<TState>() where TState : class, IState<TStateContext, TAction>, new();
    }
}
