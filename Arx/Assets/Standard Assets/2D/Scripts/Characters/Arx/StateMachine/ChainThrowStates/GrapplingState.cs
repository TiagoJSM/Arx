using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.TemplateStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.ChainThrowStates
{
    public class GrapplingState : BasePlatformerCharacterState
    {
        public override void OnStateEnter(PlatformerCharacterAction action)
        {
            base.OnStateEnter(action);
            StateController.StayStill();
        }
        public override void Perform(PlatformerCharacterAction action)
        {
            base.Perform(action);
            if (action.Jump)
            {
                StateController.ChainPull();
            }
            if (action.Move != 0)
            {
                StateController.ChainThrust();
            }
        }
    }
}
