using GenericComponents.Interfaces.States;
using GenericComponents.Interfaces.States.PlatformerCharacter;
using GenericComponents.StateMachine.States.PlatformerCharacter.TemplateStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericComponents.StateMachine.States.PlatformerCharacter
{
    public class IddleState : StandingState
    {
        public override void OnStateEnter(PlatformerCharacterAction action)
        {
            base.OnStateEnter(action);
            StateController.StartIddle();
        }

        public override void Perform(PlatformerCharacterAction action)
        {
            this.StateController.StayStill();
        }

        public override void OnStateExit(PlatformerCharacterAction action)
        {
            base.OnStateExit(action);
            StateController.StopIddle();
        }
    }
}
