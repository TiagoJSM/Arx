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
    public class RollState : DuckingState
    {
        private float _move;

        public override void OnStateEnter(PlatformerCharacterAction action)
        {
            base.OnStateEnter(action);
            _move = action.Move;
            //StateController.Roll(action.Move);
        }

        public override void Perform(PlatformerCharacterAction action)
        {
            base.Perform(action);
            StateController.Roll(_move);
        }
    }
}
