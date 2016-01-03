using GenericComponents.Interfaces.States.PlatformerCharacter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericComponents.StateMachine.States.PlatformerCharacter
{
    public class PlatformerCharacterStateManager : StateManager<IPlatformerCharacterController, PlatformerCharacterAction>
    {
        public PlatformerCharacterStateManager(IPlatformerCharacterController context)
            : base(context)
        {
            this
                .SetInitialState<IddleState>()
                    .To<MovingState>((c, a, t) => a.Move != 0 && c.IsGrounded)
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded)
                    .To<JumpingState>((c, a, t) => a.Jump && c.IsGrounded)
                    .To<DuckState>((c, a, t) => a.Vertical < 0 && c.IsGrounded);

            this
                .From<JumpingState>()
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded)
                    .To<GrabbingLedgeState>((c, a, t) => c.CanGrabLedge);

            this
                .From<FallingState>()
                    .To<GrabbingLedgeState>((c, a, t) => c.CanGrabLedge)
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Move == 0)
                    .To<MovingState>((c, a, t) => c.IsGrounded && a.Move != 0);

            this
                .From<MovingState>()
                    .To<JumpingState>((c, a, t) => a.Jump && c.IsGrounded)
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Move == 0);

            this
                .From<GrabbingLedgeState>()
                    .To<JumpingState>((c, a, t) => (a.Jump || c.VerticalSpeed > 0) && !c.GrabbingLedge)
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded && !c.GrabbingLedge);

            this
                .From<DuckState>()
                    .To<RollState>((c, a, t) => c.IsGrounded && a.Move != 0)
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Vertical >= 0)
                    .To<FallingState>((c, a, t) => !c.IsGrounded);

            this
               .From<RollState>()
                   .To<DuckState>((c, a, t) => c.IsGrounded && a.Move == 0 && t > 1);

            /*.SetTransition<FallingState>((c, a) => c.VerticalSpeed < 0 && !c.IsGrounded)
                .SetTransition<GrabbingLedgeState>((c, a) => c.CanGrabLedge)
            .Parent
            .SetTransition<JumpingState>((c, a) => a.Jump)
            .And<MovingState>((c, a) => a.Move != 0)
                .SetTransition<FallingState>((c, a) => !c.IsGrounded)
                    .SetTransition<GrabbingLedgeState>((c, a) => c.CanGrabLedge)
                        .SetTransition<FallingState>((c, a) => c.VerticalSpeed < 0 && !c.IsGrounded)
                    .Parent
                .Parent
            .Parent
                .SetTransition<JumpingState>((c, a) => a.Jump || c.VerticalSpeed > 0)
                    .SetTransition<GrabbingLedgeState>((c, a) => c.CanGrabLedge)
                        .SetTransition<FallingState>((c, a) => c.VerticalSpeed < 0 && !c.IsGrounded)
                            .SetTransition<GrabbingLedgeState>((c, a) => c.CanGrabLedge);*/
        }
    }
}
