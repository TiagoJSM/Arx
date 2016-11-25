using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericComponents.StateMachine.States.PlatformerCharacter.TemplateStates
{
    public abstract class AimingState : StandingState
    {
        public override void Perform(PlatformerCharacterAction action)
        {
            base.Perform(action);
            StateController.SetDirectionToAimDirection();
            StateController.DoAimingMove(action.Move);
            if (action.Shoot)
            {
                PerformAttack();
            }
        }

        protected abstract void PerformAttack();
    }
}
