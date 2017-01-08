using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.TemplateStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine
{
    public class LightGroundAttackState : BasePlatformerCharacterState
    {
        public override void OnStateEnter(PlatformerCharacterAction action)
        {
            base.OnStateEnter(action);
            StateController.DoMove(action.Move);
            StateController.DoPrimaryGroundAttack();
        }

        public override void OnStateExit(PlatformerCharacterAction action)
        {
            base.OnStateExit(action);
            StateController.AttackStateDone();
        }
    }
}
