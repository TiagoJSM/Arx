using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.Scripts.StateMachine
{
    public interface IState<TStateContext, TAction>
    {
        TStateContext StateController { get; set; }
        float TimeInState { get; set; }

        void OnStateEnter(TAction action);
        void Perform(TAction action);
        void OnStateExit(TAction action);
    }
}
