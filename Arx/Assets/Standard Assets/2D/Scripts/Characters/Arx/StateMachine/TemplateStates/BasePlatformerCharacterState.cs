using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericComponents.Interfaces;
using Assets.Standard_Assets.Scripts.StateMachine;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.TemplateStates
{
    public class BasePlatformerCharacterState : IState<MainPlatformerController, PlatformerCharacterAction>
    {
        public MainPlatformerController StateController { get; set; }
        public float TimeInState { get; set; }

        public virtual void OnStateEnter(PlatformerCharacterAction action)
        {
        }

        public virtual void OnStateExit(PlatformerCharacterAction action)
        {
        }

        public virtual void Perform(PlatformerCharacterAction action)
        {
        }
    }
}