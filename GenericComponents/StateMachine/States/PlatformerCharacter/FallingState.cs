using GenericComponents.Interfaces.States;
using GenericComponents.Interfaces.States.PlatformerCharacter;
using GenericComponents.StateMachine.States.PlatformerCharacter.TemplateStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericComponents.StateMachine.States.PlatformerCharacter
{
    public class FallingState : StandingState
    {
        public override IState<IPlatformerCharacterController, PlatformerCharacterAction> Perform(PlatformerCharacterAction action)
        {
            base.Perform(action);
            StateController.DoMove(action.Move);
            return this;
        }
    }
}
