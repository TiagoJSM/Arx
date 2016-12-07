using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.TemplateStates
{
    public class StandingState : BasePlatformerCharacterState
    {
        public override void OnStateEnter(PlatformerCharacterAction action)
        {
            StateController.Stand();
        }
    }
}

