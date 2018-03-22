using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.TemplateStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine
{
    public class WallJumpState : BasePlatformerCharacterState
    {
        private float _wallJumpMove;
        public override void OnStateEnter(PlatformerCharacterAction action)
        {
            base.OnStateEnter(action);
            _wallJumpMove = -StateController.WallJumpSide;
            StateController.WallJump(_wallJumpMove);
        }

        public override void Perform(PlatformerCharacterAction action)
        {
            base.Perform(action);
            StateController.JumpUp(0.9f);
        }

        public override void OnStateExit(PlatformerCharacterAction action)
        {
            base.OnStateExit(action);
            var velocity = StateController.CharacterController2D.velocity;
            velocity.y = 0.0f;
            StateController.CharacterController2D.velocity = velocity;
            var desired = StateController.DesiredMovementVelocity;
            desired.y = 0.0f;
            StateController.DesiredMovementVelocity = desired;
        }
    }
}
