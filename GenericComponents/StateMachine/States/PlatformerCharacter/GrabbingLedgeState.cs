using GenericComponents.Interfaces.States;
using GenericComponents.Interfaces.States.PlatformerCharacter;
using GenericComponents.StateMachine.States.PlatformerCharacter.TemplateStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.StateMachine.States.PlatformerCharacter
{
    public class GrabbingLedgeState : StandingState
    {
        public override void OnStateEnter(PlatformerCharacterAction action)
        {
            StateController.DoGrabLedge();
        }

        public override void Perform(PlatformerCharacterAction action)
        {
            base.Perform(action);
            var drop = action.Vertical < 0;
            if (drop)
            {
                StateController.DropLedge();
            }
        }

        public override void OnStateExit(PlatformerCharacterAction action)
        {
            StateController.DropLedge();
        }
    }
}
