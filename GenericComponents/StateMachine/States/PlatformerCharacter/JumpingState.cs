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
    public class JumpingState : StandingState
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            StateController.JumpUp();
        }

        public override IState<IPlatformerCharacterController, PlatformerCharacterAction> Perform(PlatformerCharacterAction action)
        {
            base.Perform(action);
            StateController.DoMove(action.Move);
            return this;
        }
    }
}
