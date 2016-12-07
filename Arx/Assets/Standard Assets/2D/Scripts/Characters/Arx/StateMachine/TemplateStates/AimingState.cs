using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.TemplateStates
{
    public abstract class AimingState : StandingState
    {
        public override void OnStateEnter(PlatformerCharacterAction action)
        {
            base.OnStateEnter(action);
            StateController.Aim(true);
        }

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

        public override void OnStateExit(PlatformerCharacterAction action)
        {
            base.OnStateExit(action);
            StateController.Aim(false);
        }

        protected abstract void PerformAttack();
    }
}