using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.TemplateStates;
using GenericComponents.Interfaces.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine
{
    public class JumpingState : StandingState
    {
        private const float MaxJumpTime = 0.2f;
        private bool _canJump = true;

        public override void OnStateEnter(PlatformerCharacterAction action)
        {
            _canJump = true;
        }

        public override void Perform(PlatformerCharacterAction action)
        {
            if (!action.Jump)
            {
                _canJump = false;
            }

            if (_canJump && TimeInState <= MaxJumpTime)
            {
                StateController.JumpUp(1);
            }

            base.Perform(action);
            StateController.DoMove(action.Move);
        }
    }
}