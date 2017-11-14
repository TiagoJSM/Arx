using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.TemplateStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine
{
    public class SlidingDownState : BasePlatformerCharacterState
    {
        public override void OnStateEnter(PlatformerCharacterAction action)
        {
            base.OnStateEnter(action);
            base.StateController.FlipToSlideDownDirection();
        }
    }
}