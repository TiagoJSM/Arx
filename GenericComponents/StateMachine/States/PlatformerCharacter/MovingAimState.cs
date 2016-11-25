using GenericComponents.StateMachine.States.PlatformerCharacter.TemplateStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericComponents.StateMachine.States.PlatformerCharacter
{
    public class MovingAimState : AimingState
    {
        protected override void PerformAttack()
        {
            StateController.DoPrimaryGroundAttack();
        }
    }
}
