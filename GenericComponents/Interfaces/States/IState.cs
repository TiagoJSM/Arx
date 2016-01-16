using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericComponents.Interfaces.States
{
    public interface IState<TStateContext, TAction>
    {
        TStateContext StateController { get; set; }
        float TimeInState { get; set; }

        void OnStateEnter();
        void Perform(TAction action);
        void OnStateExit();
    }
}
