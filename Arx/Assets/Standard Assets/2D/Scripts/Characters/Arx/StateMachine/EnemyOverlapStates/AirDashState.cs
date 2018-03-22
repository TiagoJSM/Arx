using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.TemplateStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.EnemyOverlapStates
{
    public class AirDashState : EnemyOverlapState
    {
        private float _move;
        private float _gravity;

        public override void OnStateEnter(PlatformerCharacterAction action)
        {
            base.OnStateEnter(action);
            _move = action.Move;
            _gravity = StateController.gravity;
            StateController.gravity = 0.0f;
            StateController.DesiredMovementVelocity = Vector2.zero;
            var velocity = StateController.CharacterController2D.velocity;
            velocity.y = 0.0f;
            StateController.CharacterController2D.velocity = velocity;
            StateController.StartDash();
        }

        public override void Perform(PlatformerCharacterAction action)
        {
            base.Perform(action);
            StateController.Dash(_move);
        }

        public override void OnStateExit(PlatformerCharacterAction action)
        {
            base.OnStateExit(action);
            StateController.gravity = _gravity;
            StateController.EndDash();
        }
    }
}
