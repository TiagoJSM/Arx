using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.TemplateStates;
using GenericComponents.Interfaces.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine
{
    public class JumpingState : StandingState
    {
        public override void OnStateEnter(PlatformerCharacterAction action)
        {
            base.OnStateEnter(action);
            StateController.JumpUp();
        }

        public override void Perform(PlatformerCharacterAction action)
        {
            base.Perform(action);
            StateController.DoMove(action.Move);
        }
    }
}