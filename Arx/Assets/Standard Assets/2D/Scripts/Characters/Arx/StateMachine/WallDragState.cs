using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.TemplateStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine
{
    public class WallDragState : BasePlatformerCharacterState
    {
        private float _gravity;
        private float _minYVelocity;

        public override void OnStateEnter(PlatformerCharacterAction action)
        {
            base.OnStateEnter(action);
            StateController.StartWallDrag();
        }

        public override void Perform(PlatformerCharacterAction action)
        {
            base.Perform(action);
            StateController.DoMove(action.Move);
        }

        public override void OnStateExit(PlatformerCharacterAction action)
        {
            base.OnStateEnter(action);
            StateController.EndWallDrag();
        }
    }
}
