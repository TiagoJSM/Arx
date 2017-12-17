using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.TemplateStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.AttackedStates
{
    public class AttackedOnGroundState : AttackedState
    {
        private float _horizontalMovement;

        public override void OnStateEnter(PlatformerCharacterAction action)
        {
            base.OnStateEnter(action);

            _horizontalMovement = 0;
            if (StateController.HitPointThisFrame != null)
            {
                _horizontalMovement = Math.Sign(StateController.transform.position.x - StateController.HitPointThisFrame.Value.x);
            }

            StateController.TakingDamage = true;
        }

        public override void Perform(PlatformerCharacterAction action)
        {
            base.Perform(action);
            StateController.DoMove(_horizontalMovement, false);
        }

        public override void OnStateExit(PlatformerCharacterAction action)
        {
            base.OnStateExit(action);
            StateController.TakingDamage = false;
        }
    }
}
