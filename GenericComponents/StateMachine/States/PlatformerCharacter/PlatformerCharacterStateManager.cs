﻿using CommonInterfaces.Weapons;
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
                    .To<MovingAimState>((c, a, t) => c.IsGrounded && a.Aiming && IsAimableWeapon(c))
                    .To<LightGroundAttackState>((c, a, t) => a.AttackType == AttackType.Primary && c.WeaponType != null)
                    .To<StrongGroundAttackState>((c, a, t) => a.AttackType == AttackType.Secundary && c.WeaponType != null)
                    .To<ChargeAttackState>((c, a, t) => a.AttackType == AttackType.Primary && c.WeaponType != null && c.IsCharging)
                    .To<GrabbingLedgeState>((c, a, t) => c.CanGrabLedge)
                    .To<MovingState>((c, a, t) => a.Move != 0 && c.IsGrounded)
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded)
                    .To<JumpingState>((c, a, t) => a.Jump && c.IsGrounded)
                    .To<DuckState>((c, a, t) => a.Vertical < 0 && c.IsGrounded);

            this
                .From<JumpingState>()
                    .To<SlidingDownState>((c, a, t) => c.SlidingDown)
                    .To<FallingAimState>((c, a, t) => !c.IsGrounded && a.Aiming && IsAimableWeapon(c))
                    .To<LightAirAttackState>((c, a, t) =>
                        a.AttackType == AttackType.Primary &&
                        c.WeaponType != null &&
                        c.WeaponType.Value == WeaponType.Sword)
                    .To<StrongAirAttackState>((c, a, t) =>
                        a.AttackType == AttackType.Secundary &&
                        c.WeaponType != null)
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded)
                    .To<GrabbingLedgeState>((c, a, t) => c.CanGrabLedge)
                    .To<IddleState>((c, a, t) => c.IsGrounded && t > 0.5)
                    .To<RopeGrabState>((c, a, t) => c.RopeFound);

            this
                .From<JumpingAimState>()
                    .To<SlidingDownState>((c, a, t) => c.SlidingDown)
                    .To<FallingAimState>((c, a, t) => !c.IsGrounded && a.Aiming && IsAimableWeapon(c))
                    .To<FallingState>((c, a, t) => !c.IsGrounded)
                    .To<GrabbingLedgeState>((c, a, t) => c.CanGrabLedge)
                    .To<IddleState>((c, a, t) => c.IsGrounded && t > 0.5)
                    .To<RopeGrabState>((c, a, t) => c.RopeFound);

            this
                .From<FallingState>()
                    .To<SlidingDownState>((c, a, t) => c.SlidingDown)
                    .To<FallingAimState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded && a.Aiming && IsAimableWeapon(c))
                    .To<LightAirAttackState>((c, a, t) => a.AttackType == AttackType.Primary && c.WeaponType != null)
                    .To<StrongAirAttackState>((c, a, t) => a.AttackType == AttackType.Secundary && c.WeaponType != null)
                    .To<GrabbingLedgeState>((c, a, t) => c.CanGrabLedge)
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Move == 0)
                    .To<MovingAimState>((c, a, t) => c.IsGrounded && a.Move != 0 && a.Aiming && IsAimableWeapon(c))
                    .To<MovingState>((c, a, t) => c.IsGrounded && a.Move != 0)
                    .To<RopeGrabState>((c, a, t) => c.RopeFound);

            this
                .From<FallingAimState>()
                    .To<LightAirAttackState>((c, a, t) => a.AttackType == AttackType.Primary && c.WeaponType != null)
                    .To<SlidingDownState>((c, a, t) => c.SlidingDown)
                    .To<GrabbingLedgeState>((c, a, t) => c.CanGrabLedge)
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded && !a.Aiming)
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Move == 0)
                    .To<MovingAimState>((c, a, t) => c.IsGrounded && a.Move != 0 && a.Aiming && IsAimableWeapon(c))
                    .To<MovingState>((c, a, t) => c.IsGrounded && a.Move != 0)
                    .To<RopeGrabState>((c, a, t) => c.RopeFound);

            this
                .From<MovingState>()
                    .To<SlidingDownState>((c, a, t) => c.SlidingDown)
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded)
                    .To<MovingAimState>((c, a, t) => a.Aiming && IsAimableWeapon(c) && c.IsGrounded)
                    .To<LightGroundAttackState>((c, a, t) => a.AttackType == AttackType.Primary && c.WeaponType != null)
                    .To<StrongGroundAttackState>((c, a, t) => a.AttackType == AttackType.Secundary && c.WeaponType != null)
                    .To<ChargeAttackState>((c, a, t) => a.AttackType == AttackType.Primary && c.WeaponType != null && c.IsCharging)
                    .To<JumpingState>((c, a, t) => a.Jump && c.IsGrounded)
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Move == 0);

            this
                .From<GrabbingLedgeState>()
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
                        a.AttackType == AttackType.Primary && c.IsAttackOver && c.WeaponType != null)
                    .To<StrongGroundAttackState>((c, a, t) =>
                        a.AttackType == AttackType.Secundary && c.IsAttackOver && c.WeaponType != null)
                    .To<IddleState>((c, a, t) => 
                        c.IsGrounded && a.Move == 0 && c.IsAttackOver)
                    .To<MovingState>((c, a, t) => c.IsGrounded && a.Move != 0 && c.IsAttackOver);

            this
                .From<StrongGroundAttackState>()
                    .To<IddleState>((c, a, t) =>
                        c.IsGrounded && a.Move == 0 && c.IsAttackOver)
                    .To<MovingState>((c, a, t) => c.IsGrounded && a.Move != 0 && c.IsAttackOver);

            this
                .From<LightAirAttackState>()
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
                        c.IsGrounded && a.Move == 0 && c.IsAttackOver)
                    .To<MovingState>((c, a, t) => c.IsGrounded && a.Move != 0 && c.IsAttackOver)
                    .To<FallingState>((c, a, t) => !c.IsGrounded && c.IsAttackOver);

            this
                .From<ChargeAttackState>()
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
                    .To<FallingState>((c, a, t) => a.ReleaseRope)
                    .To<JumpingState>((c, a, t) => a.Jump);

            this.
                From<MovingAimState>()
                .To<SlidingDownState>((c, a, t) => c.SlidingDown)
                .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded)
                .To<MovingState>((c, a, t) => !a.Aiming && IsAimableWeapon(c) && c.IsGrounded)
                .To<LightGroundAttackState>((c, a, t) => a.Shoot && c.WeaponType != null)
                .To<StrongGroundAttackState>((c, a, t) => a.AttackType == AttackType.Secundary && c.WeaponType != null)
                .To<ChargeAttackState>((c, a, t) => a.Shoot && c.WeaponType != null && c.IsCharging)
                .To<JumpingAimState>((c, a, t) => a.Jump && c.IsGrounded && a.Aiming && IsAimableWeapon(c))
                .To<JumpingState>((c, a, t) => a.Jump && c.IsGrounded);
        }

        private bool IsAimableWeapon(IPlatformerCharacterController controller)
        {
            return controller.WeaponType == WeaponType.Shoot || controller.WeaponType == WeaponType.ChainedProjectile;
        }
    }
}
