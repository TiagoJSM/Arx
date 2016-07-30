using GenericComponents.Enums;
using GenericComponents.Interfaces.States.PlatformerCharacter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.StateMachine.States.PlatformerCharacter
{
    public class PlatformerCharacterStateManager : StateManager<IPlatformerCharacterController, PlatformerCharacterAction>
    {
        public PlatformerCharacterStateManager(IPlatformerCharacterController context, float rollingDuration)
            : base(context)
        {
            this
                .SetInitialState<IddleState>()
                    .To<GrabbingLedgeState>((c, a, t) => c.CanGrabLedge)
                    .To<MovingState>((c, a, t) => a.Move != 0 && c.IsGrounded)
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded)
                    .To<JumpingState>((c, a, t) => a.Jump && c.IsGrounded)
                    .To<DuckState>((c, a, t) => a.Vertical < 0 && c.IsGrounded)
                    .To<LightAttackGroundState>((c, a, t) => a.AttackType == AttackType.Primary);

            this
                .From<JumpingState>()
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded)
                    .To<GrabbingLedgeState>((c, a, t) => c.CanGrabLedge)
                    .To<IddleState>((c, a, t) => c.IsGrounded && t > 1);

            this
                .From<FallingState>()
                    .To<GrabbingLedgeState>((c, a, t) => c.CanGrabLedge)
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Move == 0)
                    .To<MovingState>((c, a, t) => c.IsGrounded && a.Move != 0);

            this
                .From<MovingState>()
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded)
                    .To<JumpingState>((c, a, t) => a.Jump && c.IsGrounded)
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Move == 0);

            this
                .From<GrabbingLedgeState>()
                    .To<JumpingState>((c, a, t) => a.Jump)
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded && !c.GrabbingLedge);

            this
                .From<DuckState>()
                    .To<RollState>((c, a, t) => c.IsGrounded && a.Move != 0)
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Vertical >= 0 && c.CanStand)
                    .To<FallingState>((c, a, t) => !c.IsGrounded);

            this
               .From<RollState>()
                   .To<DuckState>((c, a, t) => c.IsGrounded && a.Move == 0 && (a.Vertical < 0 || !c.CanStand) && t > rollingDuration)
                   .To<IddleState>((c, a, t) => c.IsGrounded && a.Move == 0 && t > rollingDuration && c.CanStand)
                   .To<RollState>((c, a, t) => c.IsGrounded && a.Move != 0 && t > rollingDuration);

            this
                .From<LightAttackGroundState>()
                    .To<LightAttackGroundState>((c, a, t) =>
                        {
                            if(a.AttackType == AttackType.Primary && c.IsCurrentAnimationOver)
                            {
                                
                            }
                            return a.AttackType == AttackType.Primary && c.IsCurrentAnimationOver;
                        })
                    .To<IddleState>((c, a, t) => 
                        c.IsGrounded && a.Move == 0 && c.IsCurrentAnimationOver)
                    .To<MovingState>((c, a, t) => c.IsGrounded && a.Move != 0 && c.IsCurrentAnimationOver);
        }
    }
}
