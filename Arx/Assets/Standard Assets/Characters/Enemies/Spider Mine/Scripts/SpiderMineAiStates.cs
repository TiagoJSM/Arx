using Assets.Standard_Assets.Scripts.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.Characters.Enemies.Spider_Mine.Scripts
{
    public class WaitState : IState<SpiderMineAi, object>
    {
        public SpiderMineAi StateController { get; set; }
        public float TimeInState { get; set; }

        public void OnStateEnter(object action)
        {
            StateController.WaitForEnemy();
        }

        public void OnStateExit(object action)
        {
        }

        public void Perform(object action)
        {
        }
    }

    public class FollowState : IState<SpiderMineAi, object>
    {
        public SpiderMineAi StateController { get; set; }
        public float TimeInState { get; set; }

        public void OnStateEnter(object action)
        {
            StateController.FollowEnemy();
        }

        public void OnStateExit(object action)
        {
        }

        public void Perform(object action)
        {
        }
    }

    public class BlowUpCountdownState : IState<SpiderMineAi, object>
    {
        public SpiderMineAi StateController { get; set; }
        public float TimeInState { get; set; }

        public void OnStateEnter(object action)
        {
            StateController.StartCountdown();
        }

        public void OnStateExit(object action)
        {
        }

        public void Perform(object action)
        {
        }
    }

    public class BlowUpState : IState<SpiderMineAi, object>
    {
        public SpiderMineAi StateController { get; set; }
        public float TimeInState { get; set; }

        public void OnStateEnter(object action)
        {
            StateController.BlowUp();
        }

        public void OnStateExit(object action)
        {
        }

        public void Perform(object action)
        {
        }
    }
}
