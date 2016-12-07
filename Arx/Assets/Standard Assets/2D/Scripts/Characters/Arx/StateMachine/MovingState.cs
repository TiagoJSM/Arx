using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.TemplateStates;
using GenericComponents.Interfaces.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine
{
    public class MovingState : StandingState
    {
        public override void Perform(PlatformerCharacterAction action)
        {
            base.Perform(action);
            StateController.DoMove(action.Move);
        }
    }
}