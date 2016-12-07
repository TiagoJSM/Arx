using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.TemplateStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine
{
    public class RopeGrabState : BasePlatformerCharacterState
    {
        public override void OnStateEnter(PlatformerCharacterAction action)
        {
            base.OnStateEnter(action);
            StateController.GrabRope();
        }

        public override void Perform(PlatformerCharacterAction action)
        {
            base.Perform(action);
            StateController.MoveOnRope(action.Move, action.Vertical);
        }
        public override void OnStateExit(PlatformerCharacterAction action)
        {
            base.OnStateExit(action);
            StateController.LetGoRope();
        }
    }
}