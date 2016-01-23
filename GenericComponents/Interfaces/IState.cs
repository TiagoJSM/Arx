using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericComponents.Interfaces
{
    public interface IState<TStateContext, TAction>
    {
        TStateContext StateContext { get; set; }
        IStateFactory<TStateContext, TAction> StateFactory { get; set; }

        void OnStateEnter();
        IState<TStateContext, TAction> Perform(TAction action);
        void OnStateExit();
    }
}
