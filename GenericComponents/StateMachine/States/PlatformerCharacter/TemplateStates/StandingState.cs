using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericComponents.StateMachine.States.PlatformerCharacter.TemplateStates
{
    public class StandingState : BasePlatformerCharacterState
    {
        public override void OnStateEnter()
        {
            StateController.Stand();
        }
    }
}
