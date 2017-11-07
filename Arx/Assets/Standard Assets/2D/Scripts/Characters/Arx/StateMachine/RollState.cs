using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.TemplateStates;
using GenericComponents.Interfaces.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine
{
    public class RollState : DuckingState
    {
        private float _move;

        public override void OnStateEnter(PlatformerCharacterAction action)
        {
            base.OnStateEnter(action);
            _move = action.Move;
            StateController.StartRoll();
            //StateController.Roll(action.Move);
        }

        public override void Perform(PlatformerCharacterAction action)
        {
            base.Perform(action);
            StateController.Roll(_move);
        }
    }
}