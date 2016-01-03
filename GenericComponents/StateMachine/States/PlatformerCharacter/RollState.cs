using GenericComponents.Interfaces.States;
using GenericComponents.Interfaces.States.PlatformerCharacter;
using GenericComponents.StateMachine.States.PlatformerCharacter.TemplateStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericComponents.StateMachine.States.PlatformerCharacter
{
    public class RollState : DuckingState
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            StateController.DoMove(1);
        }
    }
}
