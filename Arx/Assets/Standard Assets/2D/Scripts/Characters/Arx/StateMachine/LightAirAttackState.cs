using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.TemplateStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine
{
    public class LightAirAttackState : BasePlatformerCharacterState
    {
        public override void OnStateEnter(PlatformerCharacterAction action)
        {
            base.OnStateEnter(action);
            StateController.DoPrimaryAirAttack();
        }

        public override void Perform(PlatformerCharacterAction action)
        {
            base.Perform(action);
            if (StateController.IsGrounded)
            {
                StateController.StayStill();
            }
        }
    }
}