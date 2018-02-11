using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.Scripts.StateMachine
{
    public class WhenTransitionToConfig<TStateController, TAction>
    {
        public Type Type { get; private set; }
        public Action<TStateController, TAction> Action { get; private set; }

        public WhenTransitionToConfig(Type type, Action<TStateController, TAction> action)
        {
            Type = type;
            Action = action;
        }
    }
}
