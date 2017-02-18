using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.AttackedStates;
using CommonInterfaces.Weapons;
using GenericComponents.Enums;
using GenericComponents.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine
{
    public class PlatformerCharacterStateManager : StateManager<IPlatformerCharacterController, PlatformerCharacterAction>
    {
        public PlatformerCharacterStateManager(IPlatformerCharacterController context, float rollingDuration)
            : base(context)
        {
            this
                .SetInitialState<IddleState>()
                    .To<AttackedOnGroundState>((c, a, t) => c.AttackedThisFrame)
                    .To<MovingAimState>((c, a, t) => c.IsGrounded && a.Aiming)
                    .To<LightGroundAttackState>((c, a, t) => 
                        a.AttackType == AttackType.Primary && c.Attacking && c.WeaponType != null)
                    .To<StrongGroundAttackState>((c, a, t) => a.AttackType == AttackType.Secundary && c.Attacking && c.WeaponType != null)
                    .To<ChargeAttackState>((c, a, t) => a.AttackType == AttackType.Primary && c.WeaponType != null && c.IsCharging)
                    .To<GrabbingLedgeState>((c, a, t) => c.CanGrabLedge)
                    .To<MovingState>((c, a, t) => a.Move != 0 && c.IsGrounded)
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded)
                    .To<JumpingState>((c, a, t) => a.Jump && c.IsGrounded)
                    .To<DuckState>((c, a, t) => a.Vertical < 0 && c.IsGrounded);

            this
                .From<JumpingState>()
                    .To<AttackedOnAirState>((c, a, t) => c.AttackedThisFrame)
                    .To<SlidingDownState>((c, a, t) => c.SlidingDown)
                    .To<FallingAimState>((c, a, t) => !c.IsGrounded && a.Aiming)
                    .To<LightAirAttackState>((c, a, t) =>
                        a.AttackType == AttackType.Primary &&
                        c.WeaponType != null &&
                        c.Attacking &&
                        c.WeaponType.Value == WeaponType.Sword)
                    .To<StrongAirAttackState>((c, a, t) =>
                        a.AttackType == AttackType.Secundary &&
                        c.Attacking &&
                        c.WeaponType != null)
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded)
                    .To<GrabbingLedgeState>((c, a, t) => c.CanGrabLedge)
                    .To<IddleState>((c, a, t) => c.IsGrounded && t > 0.5)
                    .To<RopeGrabState>((c, a, t) => c.RopeFound);

            this
                .From<JumpingAimState>()
                    .To<AttackedOnAirState>((c, a, t) => c.AttackedThisFrame)
                    .To<SlidingDownState>((c, a, t) => c.SlidingDown)
                    .To<FallingAimState>((c, a, t) => !c.IsGrounded && a.Aiming)
                    .To<FallingState>((c, a, t) => !c.IsGrounded)
                    .To<ThrowState>((c, a, t) => a.Throw)
                    .To<GrabbingLedgeState>((c, a, t) => c.CanGrabLedge)
                    .To<IddleState>((c, a, t) => c.IsGrounded && t > 0.5)
                    .To<RopeGrabState>((c, a, t) => c.RopeFound);

            this
                .From<FallingState>()
                    .To<DamagedAndMovedState>((c, a, t) => c.SafeSpot.HasValue)
                    .To<AttackedOnAirState>((c, a, t) => c.AttackedThisFrame)
                    .To<SlidingDownState>((c, a, t) => c.SlidingDown)
                    .To<FallingAimState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded && a.Aiming)
                    .To<LightAirAttackState>((c, a, t) => a.AttackType == AttackType.Primary && c.Attacking && c.WeaponType != null)
                    .To<StrongAirAttackState>((c, a, t) => a.AttackType == AttackType.Secundary && c.Attacking && c.WeaponType != null)
                    .To<GrabbingLedgeState>((c, a, t) => c.CanGrabLedge)
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Move == 0)
                    .To<MovingAimState>((c, a, t) => c.IsGrounded && a.Move != 0 && a.Aiming)
                    .To<MovingState>((c, a, t) => c.IsGrounded && a.Move != 0)
                    .To<RopeGrabState>((c, a, t) => c.RopeFound);

            this
                .From<FallingAimState>()
                    .To<AttackedOnAirState>((c, a, t) => c.AttackedThisFrame)
                    .To<SlidingDownState>((c, a, t) => c.SlidingDown)
                    .To<GrabbingLedgeState>((c, a, t) => c.CanGrabLedge)
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded && !a.Aiming)
                    .To<ThrowState>((c, a, t) => a.Throw)
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Move == 0)
                    .To<MovingAimState>((c, a, t) => c.IsGrounded && a.Move != 0 && a.Aiming)
                    .To<MovingState>((c, a, t) => c.IsGrounded && a.Move != 0)
                    .To<RopeGrabState>((c, a, t) => c.RopeFound);

            this
                .From<MovingState>()
                    .To<DamagedAndMovedState>((c, a, t) => c.SafeSpot.HasValue)
                    .To<AttackedOnGroundState>((c, a, t) => c.AttackedThisFrame)
                    .To<SlidingDownState>((c, a, t) => c.SlidingDown)
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded)
                    .To<LightGroundAttackState>((c, a, t) => a.AttackType == AttackType.Primary && c.Attacking)
                    .To<StrongGroundAttackState>((c, a, t) => a.AttackType == AttackType.Secundary && c.Attacking)
                    .To<MovingAimState>((c, a, t) => a.Aiming && c.IsGrounded)
                    .To<ChargeAttackState>((c, a, t) => a.AttackType == AttackType.Primary && c.WeaponType != null && c.IsCharging)
                    .To<JumpingState>((c, a, t) => a.Jump && c.IsGrounded)
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Move == 0)
                    .To<PushState>((c, a, t) => c.IsGrounded && c.Pushable != null && a.Move != 0);

            this
                .From<GrabbingLedgeState>()
                    .To<AttackedOnGrabbingState>((c, a, t) => c.AttackedThisFrame)
                    .To<JumpingState>((c, a, t) => a.Jump)
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded && !c.GrabbingLedge);

            this
                .From<DuckState>()
                    .To<SlidingDownState>((c, a, t) => c.SlidingDown)
                    .To<RollState>((c, a, t) => c.IsGrounded && a.Roll)
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Vertical >= 0 && c.CanStand)
                    .To<FallingState>((c, a, t) => !c.IsGrounded);

            this
                .From<RollState>()
                    .To<SlidingDownState>((c, a, t) => c.SlidingDown)
                    .To<DuckState>((c, a, t) => c.IsGrounded && a.Move == 0 && (a.Vertical < 0 || !c.CanStand) && t > rollingDuration)
                    .To<FallingState>((c, a, t) => !c.IsGrounded)
                    .To<RollState>((c, a, t) => c.IsGrounded && a.Roll && t > rollingDuration)
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Move == 0 && t > rollingDuration && c.CanStand)
                    .To<MovingState>((c, a, t) => c.IsGrounded && a.Move != 0 && t > rollingDuration && c.CanStand);

            this
                .From<LightGroundAttackState>()
                    .To<LightGroundAttackState>((c, a, t) =>
                        a.AttackType == AttackType.Primary && c.Attacking && c.WeaponType != null)
                    .To<StrongGroundAttackState>((c, a, t) =>
                        a.AttackType == AttackType.Secundary && c.Attacking && c.WeaponType != null)
                    .To<IddleState>((c, a, t) =>
                        a.AttackType == AttackType.None && c.IsGrounded && a.Move == 0 && !c.Attacking)
                    .To<MovingState>((c, a, t) =>
                        a.AttackType == AttackType.None && c.IsGrounded && a.Move != 0 && c.IsAttackOver);

            this
                .From<StrongGroundAttackState>()
                    .To<AttackedOnGroundState>((c, a, t) => c.AttackedThisFrame)
                    .To<IddleState>((c, a, t) =>
                        a.AttackType == AttackType.None && c.IsGrounded && a.Move == 0 && !c.Attacking)
                    .To<MovingState>((c, a, t) =>
                        a.AttackType == AttackType.None && c.IsGrounded && a.Move != 0 && !c.Attacking);

            this
                .From<LightAirAttackState>()
                    .To<AttackedOnGroundState>((c, a, t) => c.AttackedThisFrame)
                    .To<LightAirAttackState>((c, a, t) =>
                        a.AttackType == AttackType.Primary && c.IsAttackOver && c.WeaponType != null && !c.IsGrounded)
                    .To<IddleState>((c, a, t) =>
                        c.IsGrounded && a.Move == 0 && c.IsAttackOver)
                    .To<MovingState>((c, a, t) => c.IsGrounded && a.Move != 0 && c.IsAttackOver)
                    .To<FallingState>((c, a, t) => !c.IsGrounded && c.IsAttackOver);

            this
                .From<StrongAirAttackState>()
                    .To<StrongAirAttackState>((c, a, t) =>
                        a.AttackType == AttackType.Secundary && c.IsAttackOver && c.WeaponType != null)
                    .To<IddleState>((c, a, t) =>
                        c.IsGrounded && a.Move == 0)
                    .To<MovingState>((c, a, t) => c.IsGrounded && a.Move != 0);/*
                    .To<FallingState>((c, a, t) => !c.IsGrounded && c.IsAttackOver);*/

            this
                .From<ChargeAttackState>()
                    .To<AttackedOnGroundState>((c, a, t) => c.AttackedThisFrame)
                    .To<ReleaseChargeAttackState>((c, a, t) => !c.IsCharging && c.WeaponType != null);

            this
                .From<ReleaseChargeAttackState>()
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Move == 0 && c.IsAttackOver);

            this
                .From<SlidingDownState>()
                    .To<MovingState>((c, a, t) => a.Move != 0 && c.IsGrounded && !c.SlidingDown)
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Move == 0 && !c.SlidingDown)
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded && !c.SlidingDown);

            this.
                From<RopeGrabState>()
                    .To<AttackedOnGrabbingState>((c, a, t) => c.AttackedThisFrame)
                    .To<FallingState>((c, a, t) => a.ReleaseRope)
                    .To<JumpingState>((c, a, t) => a.Jump);

            this.
                From<MovingAimState>()
                .To<SlidingDownState>((c, a, t) => c.SlidingDown)
                .To<AttackedOnGroundState>((c, a, t) => c.AttackedThisFrame)
                .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded)
                .To<MovingState>((c, a, t) => !a.Aiming && c.IsGrounded)
                .To<ThrowState>((c, a, t) => a.Throw)
                .To<ChargeAttackState>((c, a, t) => a.Shoot && c.WeaponType != null && c.IsCharging)
                .To<JumpingAimState>((c, a, t) => a.Jump && c.IsGrounded && a.Aiming)
                .To<JumpingState>((c, a, t) => a.Jump && c.IsGrounded);

            this.
                From<ThrowState>()
                    .To<AttackedOnGroundState>((c, a, t) => c.AttackedThisFrame)
                    .To<GrappledState>((c, a, t) => c.GrappleRope)
                    .To<FallingAimState>((c, a, t) => c.IsAttackOver && !c.IsGrounded)
                    .To<MovingAimState>((c, a, t) => c.IsAttackOver && c.IsGrounded);

            this.
                From<GrappledState>()
                    .To<FallingState>((c, a, t) => a.ReleaseRope)
                    .To<JumpingState>((c, a, t) => a.Jump);

            this
                .From<PushState>()
                    .To<AttackedOnGroundState>((c, a, t) => c.AttackedThisFrame)
                    .To<SlidingDownState>((c, a, t) => c.SlidingDown)
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded)
                    .To<JumpingState>((c, a, t) => a.Jump && c.IsGrounded)
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Move == 0)
                    .To<MovingState>((c, a, t) => c.IsGrounded && c.Pushable == null && a.Move != 0);

            this
                .From<DamagedAndMovedState>()
                    .To<IddleState>((c, a, t) => c.SafeSpot == null);

            this
                .From<AttackedOnAirState>()
                    .To<IddleState>((c, a, t) => t > 2);

            this
                .From<AttackedOnGroundState>()
                    .To<IddleState>((c, a, t) => t > 1);

            this
                .From<AttackedOnGrabbingState>()
                    .To<IddleState>((c, a, t) => t > 1);
        }
    }
}