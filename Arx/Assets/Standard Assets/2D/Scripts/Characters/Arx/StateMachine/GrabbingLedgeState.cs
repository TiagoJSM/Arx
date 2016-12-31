using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.TemplateStates;
using GenericComponents.Interfaces.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine
{
    public class GrabbingLedgeState : StandingState
    {
        public override void OnStateEnter(PlatformerCharacterAction action)
        {
            StateController.StayStill();
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