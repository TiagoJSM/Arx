using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.TemplateStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine
{
    public class SprintJumpState : StandingState
    {
        private const float MaxJumpTime = 0.2f;

        private float _move;

        public override void OnStateEnter(PlatformerCharacterAction action)
        {
            _move = action.Move;
            StateController.StartSprintJump();
        }

        public override void Perform(PlatformerCharacterAction action)
        {
            base.Perform(action);
            if (TimeInState < 0.5)
            {
                StateController.JumpUp(0.3f);
            }
            StateController.SprintJumpMovement(_move);
        }

        public override void OnStateExit(PlatformerCharacterAction action)
        {
            base.OnStateExit(action);
            StateController.StopSprintJump();
        }
    }
}
