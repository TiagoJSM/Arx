using ArxGame.Components.Ai;
using GenericComponents.Interfaces.States;
using GenericComponents.Interfaces.States.PlatformerCharacter;
using GenericComponents.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArxGame.StateMachine
{
    public class NightmareHunterGateAiStateManager : StateManager<INightmareHunterGateLiftAiControl, object>
    {
        private class FollowState : IState<INightmareHunterGateLiftAiControl, object>
        {
            public INightmareHunterGateLiftAiControl StateController { get; set; }
            public float TimeInState { get; set; }

            public void OnStateEnter(object action)
            {
                
            }

            public void OnStateExit(object action)
            {
                StateController.StopMoving();
            }

            public void Perform(object action)
            {
                StateController.MoveToTarget();
            }
        }

        private class ReachedGateState : IState<INightmareHunterGateLiftAiControl, object>
        {
            public INightmareHunterGateLiftAiControl StateController { get; set; }
            public float TimeInState { get; set; }

            public void OnStateEnter(object action)
            {
                StateController.KnockGate();
            }

            public void OnStateExit(object action)
            {
            }

            public void Perform(object action)
            {
            }
        }

        private class ReachedTargetState : IState<INightmareHunterGateLiftAiControl, object>
        {
            public INightmareHunterGateLiftAiControl StateController { get; set; }
            public float TimeInState { get; set; }

            public void OnStateEnter(object action)
            {
                StateController.AttackTarget();
            }

            public void OnStateExit(object action)
            {
            }

            public void Perform(object action)
            {
            }
        }

        private class MoveAwayState : IState<INightmareHunterGateLiftAiControl, object>
        {
            public INightmareHunterGateLiftAiControl StateController { get; set; }
            public float TimeInState { get; set; }

            public void OnStateEnter(object action)
            {
                StateController.MoveAway();
            }

            public void OnStateExit(object action)
            {
            }

            public void Perform(object action)
            {
            }
        }

        private class DeathState : IState<INightmareHunterGateLiftAiControl, object>
        {
            public INightmareHunterGateLiftAiControl StateController { get; set; }
            public float TimeInState { get; set; }

            public void OnStateEnter(object action)
            {
                StateController.Die();
            }

            public void OnStateExit(object action)
            {
            }

            public void Perform(object action)
            {
            }
        }

        public NightmareHunterGateAiStateManager(INightmareHunterGateLiftAiControl controller)
            : base(controller)
        {
            this
                .SetInitialState<FollowState>()
                    .To<ReachedGateState>((c, a, t) => c.ReachedGate)
                    .To<ReachedTargetState>((c, a, t) => c.ReachedTarget);

            this
                .From<ReachedGateState>()
                    .To<MoveAwayState>((c, a, t) => t > 4);

            this
                .From<MoveAwayState>()
                    .To<DeathState>((c, a, t) => t > 4);
        }
    }
}
