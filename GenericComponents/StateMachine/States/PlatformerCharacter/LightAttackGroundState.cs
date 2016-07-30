using GenericComponents.StateMachine.States.PlatformerCharacter.TemplateStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericComponents.StateMachine.States.PlatformerCharacter
{
    public class LightAttackGroundState : BasePlatformerCharacterState
    {
        public override void OnStateEnter(PlatformerCharacterAction action)
        {
            base.OnStateEnter(action);
            StateController.DoPrimaryAttack();
        }

        public override void OnStateExit(PlatformerCharacterAction action)
        {
            base.OnStateExit(action);
            StateController.AttackIsOver();
        }
    }
}
