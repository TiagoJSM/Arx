using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.TemplateStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.EnemyOverlapStates
{
    public class StingDashState : EnemyOverlapState
    {
        private float _move;

        public override void OnStateEnter(PlatformerCharacterAction action)
        {
            base.OnStateEnter(action);
            _move = action.Move;
            StateController.StartStingDash();
        }

        public override void Perform(PlatformerCharacterAction action)
        {
            base.Perform(action);
            StateController.StingDashMovement(_move);
        }

        public override void OnStateExit(PlatformerCharacterAction action)
        {
            base.OnStateExit(action);
            StateController.StopStingDash();
        }
    }
}
