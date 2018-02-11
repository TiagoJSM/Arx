using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.Scripts.StateMachine
{
    public interface IStateFactory<TController, TAction>
    {
        TState State<TState>() where TState : class, IState<TController, TAction>, new();
        StateContainer<TController, TAction> GetContainer<TState>() where TState : class, IState<TController, TAction>, new();
    }
}
