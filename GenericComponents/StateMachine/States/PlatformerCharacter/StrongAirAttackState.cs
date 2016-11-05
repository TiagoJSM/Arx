using GenericComponents.StateMachine.States.PlatformerCharacter;
using GenericComponents.StateMachine.States.PlatformerCharacter.TemplateStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericComponents.StateMachine.States.PlatformerCharacter
{
    public class StrongAirAttackState : BasePlatformerCharacterState
    {
        public override void OnStateEnter(PlatformerCharacterAction action)
        {
            base.OnStateEnter(action);
            StateController.DoSecundaryAirAttack();
        }

        public override void Perform(PlatformerCharacterAction action)
        {
            base.Perform(action);
            if (StateController.FrameHits.Any())
            {
                StateController.StopAirSlash();
            }
        }
    }
}
