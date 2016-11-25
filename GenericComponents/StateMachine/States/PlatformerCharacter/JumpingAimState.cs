using GenericComponents.StateMachine.States.PlatformerCharacter.TemplateStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericComponents.StateMachine.States.PlatformerCharacter
{
    public class JumpingAimState : AimingState
    {
        public override void OnStateEnter(PlatformerCharacterAction action)
        {
            base.OnStateEnter(action);
            StateController.JumpUp();
        }

        protected override void PerformAttack()
        {
            StateController.DoPrimaryAirAttack();
        }
    }
}
