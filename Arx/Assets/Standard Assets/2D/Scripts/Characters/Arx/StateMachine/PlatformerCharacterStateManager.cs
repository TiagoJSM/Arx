using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.AttackedStates;
using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.ChainThrowStates;
using Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine.ThrowStates;
using Assets.Standard_Assets.Scripts.StateMachine;
using Assets.Standard_Assets.Weapons;
using GenericComponents.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx.StateMachine
{
    public class PlatformerCharacterStateManager : StateManager<MainPlatformerController, PlatformerCharacterAction>
    {
        public PlatformerCharacterStateManager(MainPlatformerController context, float rollingDuration)
            : base(context)
        {
            this
                .SetInitialState<IddleState>()
                    .To<AttackedOnGroundState>((c, a, t) => c.AttackedThisFrame)
                    .To<RollState>((c, a, t) => c.IsGrounded && a.Roll)
                    .To<MovingAimShootState>((c, a, t) => c.IsGrounded && a.Aiming && c.ShootWeaponEquipped)
                    .To<MovingAimThrowState>((c, a, t) => c.IsGrounded && a.Aiming && c.ThrowWeaponEquipped)
                    .To<MovingAimChainThrowState>((c, a, t) => c.IsGrounded && a.Aiming && c.ChainThrowWeaponEquipped)
                    .To<GroundAttackState>((c, a, t) => 
                        a.AttackType != AttackType.None && c.Attacking && c.WeaponType != null)
                    .To<ChargeAttackState>((c, a, t) => a.AttackType == AttackType.Primary && c.WeaponType != null && c.IsCharging)
                    .To<GrabbingLedgeState>((c, a, t) => c.CanGrabLedge)
                    .To<MovingState>((c, a, t) => a.Move != 0 && c.IsGrounded)
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded)
                    .To<JumpingState>((c, a, t) => a.Jump && c.IsGrounded)
                    .To<DuckState>((c, a, t) => a.Vertical < 0 && c.IsGrounded)
                    .To<LadderGrabState>((c, a, t) => c.LadderFound && a.GrabLadder);

            this
                .FromAny()
                    .To<DamagedAndMovedState>((c, a, t) => c.SafeSpot.HasValue && this.CurrentState.GetType() != typeof(DamagedAndMovedState));

            this
                .From<JumpingState>()
                    .To<AttackedOnAirState>((c, a, t) => c.AttackedThisFrame)
                    .To<SlidingDownState>((c, a, t) => c.SlidingDown)
                    .To<FallingAimShootState>((c, a, t) => !c.IsGrounded && a.Aiming)
                    .To<LightAirAttackState>((c, a, t) =>
                        a.AttackType == AttackType.Primary &&
                        c.WeaponType != null &&
                        c.Attacking &&
                        c.WeaponType.Value == WeaponType.Sword)
                    .To<StrongAirAttackState>((c, a, t) =>
                        a.AttackType == AttackType.Secundary &&
                        c.Attacking &&
                        c.WeaponType != null)
                    .To<FallingState>((c, a, t) => (c.VerticalSpeed < 0 && !c.IsGrounded) || c.CollidesAbove)
                    .To<GrabbingLedgeState>((c, a, t) => c.CanGrabLedge)
                    .To<IddleState>((c, a, t) => c.IsGrounded && t > 0.5)
                    .To<RopeGrabState>((c, a, t) => c.RopeFound)
                    .To<LadderGrabState>((c, a, t) => c.LadderFound && a.GrabLadder);

            this
                .From<JumpingAimShootState>()
                    .To<AttackedOnAirState>((c, a, t) => c.AttackedThisFrame)
                    .To<SlidingDownState>((c, a, t) => c.SlidingDown)
                    .To<FallingAimShootState>((c, a, t) => !c.IsGrounded && a.Aiming)
                    .To<FallingState>((c, a, t) => !c.IsGrounded)
                    //.To<ThrowState>((c, a, t) => a.Throw)
                    .To<GrabbingLedgeState>((c, a, t) => c.CanGrabLedge)
                    .To<IddleState>((c, a, t) => c.IsGrounded && t > 0.5)
                    .To<RopeGrabState>((c, a, t) => c.RopeFound)
                    .To<LadderGrabState>((c, a, t) => c.LadderFound && a.GrabLadder);

            this
                .From<FallingState>()
                    .To<AttackedOnAirState>((c, a, t) => c.AttackedThisFrame)
                    .To<SlidingDownState>((c, a, t) => c.SlidingDown)
                    .WhenTransitionTo<SlidingDownState>((c, a) => c.OnLanded())
                    .To<FallingAimShootState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded && a.Aiming)
                    .To<LightAirAttackState>((c, a, t) => a.AttackType == AttackType.Primary && c.Attacking && c.WeaponType != null)
                    .To<StrongAirAttackState>((c, a, t) => 
                        a.AttackType == AttackType.Secundary && c.Attacking && c.WeaponType != null)
                    .To<GrabbingLedgeState>((c, a, t) => c.CanGrabLedge)
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Move == 0)
                    .WhenTransitionTo<IddleState>((c, a) => c.OnLanded())
                    .To<MovingAimShootState>((c, a, t) => c.IsGrounded && a.Move != 0 && a.Aiming && c.ShootWeaponEquipped)
                    .To<MovingAimThrowState>((c, a, t) => c.IsGrounded && a.Move != 0 && a.Aiming && c.ThrowWeaponEquipped)
                    .To<MovingAimChainThrowState>((c, a, t) => c.IsGrounded && a.Move != 0 && a.Aiming && c.ChainThrowWeaponEquipped)
                    .WhenTransitionTo<MovingAimShootState>((c, a) => c.OnLanded())
                    .To<MovingState>((c, a, t) => c.IsGrounded && a.Move != 0)
                    .WhenTransitionTo<MovingState>((c, a) => c.OnLanded())
                    .To<RopeGrabState>((c, a, t) => c.RopeFound)
                    .To<LadderGrabState>((c, a, t) => c.LadderFound && a.GrabLadder);

            this
                .From<FallingAimShootState>()
                    .To<AttackedOnAirState>((c, a, t) => c.AttackedThisFrame)
                    .To<SlidingDownState>((c, a, t) => c.SlidingDown)
                    .WhenTransitionTo<SlidingDownState>((c, a) => c.OnLanded())
                    .To<GrabbingLedgeState>((c, a, t) => c.CanGrabLedge)
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded && !a.Aiming)
                    //.To<ThrowState>((c, a, t) => a.Throw)
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Move == 0)
                    .WhenTransitionTo<IddleState>((c, a) => c.OnLanded())
                    .To<MovingAimShootState>((c, a, t) => c.IsGrounded && a.Move != 0 && a.Aiming)
                    .WhenTransitionTo<MovingAimShootState>((c, a) => c.OnLanded())
                    .To<MovingState>((c, a, t) => c.IsGrounded && a.Move != 0)
                    .WhenTransitionTo<MovingState>((c, a) => c.OnLanded())
                    .To<RopeGrabState>((c, a, t) => c.RopeFound)
                    .To<LadderGrabState>((c, a, t) => c.LadderFound && a.GrabLadder);

            this
                .From<MovingState>()
                    .To<AttackedOnGroundState>((c, a, t) => c.AttackedThisFrame)
                    .To<SlidingDownState>((c, a, t) => c.SlidingDown)
                    .To<GhostJumpFallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded)
                    .To<GroundAttackState>((c, a, t) => a.AttackType != AttackType.None && c.Attacking)
                    .To<RollState>((c, a, t) => c.IsGrounded && a.Roll)
                    .To<MovingAimShootState>((c, a, t) => c.IsGrounded && a.Move != 0 && a.Aiming && c.ShootWeaponEquipped)
                    .To<MovingAimThrowState>((c, a, t) => c.IsGrounded && a.Move != 0 && a.Aiming && c.ThrowWeaponEquipped)
                    .To<MovingAimChainThrowState>((c, a, t) => c.IsGrounded && a.Move != 0 && a.Aiming && c.ChainThrowWeaponEquipped)
                    .To<ChargeAttackState>((c, a, t) => a.AttackType == AttackType.Primary && c.WeaponType != null && c.IsCharging)
                    .To<JumpingState>((c, a, t) => a.Jump && c.IsGrounded)
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Move == 0)
                    .To<PushState>((c, a, t) => c.IsGrounded && c.Pushable != null && a.Move != 0)
                    .To<LadderGrabState>((c, a, t) => c.LadderFound && a.GrabLadder);

            this
                .From<GhostJumpFallingState>()
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded && t > 0.1f)
                    .To<JumpingState>((c, a, t) => a.Jump)
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Move == 0)
                    .WhenTransitionTo<IddleState>((c, a) => c.OnLanded())
                    .To<MovingAimShootState>((c, a, t) => c.IsGrounded && a.Move != 0 && a.Aiming && c.ShootWeaponEquipped)
                    .WhenTransitionTo<MovingAimShootState>((c, a) => c.OnLanded())
                    .To<MovingAimThrowState>((c, a, t) => c.IsGrounded && a.Move != 0 && a.Aiming && c.ThrowWeaponEquipped)
                    .WhenTransitionTo<MovingAimThrowState>((c, a) => c.OnLanded())
                    .To<MovingAimChainThrowState>((c, a, t) => c.IsGrounded && a.Move != 0 && a.Aiming && c.ChainThrowWeaponEquipped)
                    .WhenTransitionTo<MovingAimChainThrowState>((c, a) => c.OnLanded());

            this
                .From<GrabbingLedgeState>()
                    .To<AttackedOnGrabbingState>((c, a, t) => c.AttackedThisFrame)
                    .To<JumpingState>((c, a, t) => a.JumpOnLedge)
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded && !c.GrabbingLedge);

            this
                .From<DuckState>()
                    .To<AttackedOnGroundState>((c, a, t) => c.AttackedThisFrame)
                    .To<SlidingDownState>((c, a, t) => c.SlidingDown)
                    .To<RollState>((c, a, t) => c.IsGrounded && a.Roll)
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Vertical >= 0 && c.CanStand)
                    .To<FallingState>((c, a, t) => !c.IsGrounded);

            this
                .From<RollState>()
                    .To<AttackedOnGroundState>((c, a, t) => c.AttackedThisFrame)
                    .To<SlidingDownState>((c, a, t) => c.SlidingDown)
                    .To<DuckState>((c, a, t) => c.IsGrounded && !c.CanStand && t > rollingDuration)
                    .To<FallingState>((c, a, t) => !c.IsGrounded && t > rollingDuration)
                    .To<RollState>((c, a, t) => c.IsGrounded && a.Roll && t > rollingDuration)
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Move == 0 && t > rollingDuration && c.CanStand)
                    .To<MovingState>((c, a, t) => c.IsGrounded && a.Move != 0 && t > rollingDuration && c.CanStand);

            this
                .From<GroundAttackState>()
                    .To<AttackedOnGroundState>((c, a, t) => c.AttackedThisFrame)
                    .To<RollState>((c, a, t) => c.IsGrounded && a.RollAfterAttack && !c.Attacking)
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Move == 0 && !c.Attacking)
                    .To<MovingState>((c, a, t) => c.IsGrounded && a.Move != 0 && !c.Attacking);

            this
                .From<LightAirAttackState>()
                    .To<AttackedOnGroundState>((c, a, t) => c.AttackedThisFrame)
                    .To<LightAirAttackState>((c, a, t) =>
                        a.AttackType == AttackType.Primary && !c.Attacking && c.WeaponType != null && !c.IsGrounded)
                    .To<IddleState>((c, a, t) =>
                        c.IsGrounded && a.Move == 0 && !c.Attacking)
                    .To<MovingState>((c, a, t) => c.IsGrounded && a.Move != 0 && !c.Attacking)
                    .To<FallingState>((c, a, t) => !c.IsGrounded && !c.Attacking);

            this
                .From<StrongAirAttackState>()
                    .To<StrongAirAttackState>((c, a, t) =>
                        a.AttackType == AttackType.Secundary && !c.Attacking && c.WeaponType != null)
                    .To<IddleState>((c, a, t) =>
                        c.IsGrounded && a.Move == 0)
                    .WhenTransitionTo<IddleState>((c, a) => c.OnAirSlashLanded())
                    .To<MovingState>((c, a, t) => c.IsGrounded && a.Move != 0)
                    .WhenTransitionTo<MovingState>((c, a) => c.OnAirSlashLanded())
                    .To<FallingState>((c, a, t) => !c.IsGrounded && !c.Attacking);

            this
                .From<ChargeAttackState>()
                    .To<AttackedOnGroundState>((c, a, t) => c.AttackedThisFrame)
                    .To<ReleaseChargeAttackState>((c, a, t) => !c.IsCharging && c.WeaponType != null);

            this
                .From<ReleaseChargeAttackState>()
                    .To<IddleState>((c, a, t) => c.IsGrounded && a.Move == 0 && !c.Attacking);

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
                From<MovingAimShootState>()
                .And<MovingAimThrowState>()
                .And<MovingAimChainThrowState>()
                    .To<SlidingDownState>((c, a, t) => c.SlidingDown)
                    .To<AttackedOnGroundState>((c, a, t) => c.AttackedThisFrame)
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded)
                    .To<RollState>((c, a, t) => c.IsGrounded && a.Roll)
                    .To<MovingState>((c, a, t) => !a.Aiming && c.IsGrounded)
                    //.To<ThrowState>((c, a, t) => a.Throw)
                    .To<ChargeAttackState>((c, a, t) => a.Shoot && c.WeaponType != null && c.IsCharging)
                    .To<JumpingAimShootState>((c, a, t) => a.Jump && c.IsGrounded && a.Aiming)
                    .To<JumpingState>((c, a, t) => a.Jump && c.IsGrounded)
                    .To<LadderGrabState>((c, a, t) => c.LadderFound && a.GrabLadder);

            this.
                From<ThrowState>()
                    .To<AttackedOnGroundState>((c, a, t) => c.AttackedThisFrame)
                    .To<FallingAimShootState>((c, a, t) => !c.Attacking && !c.IsGrounded)
                    .To<MovingAimShootState>((c, a, t) => !c.Attacking && c.IsGrounded);

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
                    .To<IddleState>((c, a, t) => t > 0.5);

            this
                .From<AttackedOnGroundState>()
                    .To<IddleState>((c, a, t) => t > 0.5);

            this
                .From<AttackedOnGrabbingState>()
                    .To<IddleState>((c, a, t) => t > 1);

            this
                .From<LadderGrabState>()
                    .To<AttackedOnAirState>((c, a, t) => c.AttackedThisFrame)
                    .To<JumpingState>((c, a, t) => a.Jump)
                    .To<FallingState>((c, a, t) => !c.LadderFound && !c.IsGrounded)
                    .To<IddleState>((c, a, t) => !c.LadderFound);

            this
                .From<MovingAimChainThrowState>()
                    .To<ThrowChainState>((c, a, t) => c.ChainThrowCombat.Weapon.Throwing)
                    .To<GrapplingState>((c, a, t) => c.Grappling);

            this
                .From<ThrowChainState>()
                    .To<GrapplingState>((c, a, t) => c.Grappling)
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded && c.ChainThrowCombat.Weapon.ReadyToThrow)
                    .To<IddleState>((c, a, t) => c.IsGrounded && c.ChainThrowCombat.Weapon.ReadyToThrow);

            this
                .From<GrapplingState>()
                    .To<PullingEnemyState>((c, a, t) => c.ChainThrowCombat.ChainPulling)
                    .To<ChainThrustingState>((c, a, t) => c.ChainThrowCombat.ChainThrusting)
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded && (t > 2 || !c.Grappling))
                    .To<IddleState>((c, a, t) => c.IsGrounded && (t > 2 || !c.Grappling));

            this
                .From<PullingEnemyState>()
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded && !c.ChainThrowCombat.ChainPulling)
                    .To<IddleState>((c, a, t) => c.IsGrounded && !c.ChainThrowCombat.ChainPulling);

            this
                .From<ChainThrustingState>()
                    .To<FallingState>((c, a, t) => c.VerticalSpeed < 0 && !c.IsGrounded && !c.ChainThrowCombat.ChainThrusting)
                    .To<IddleState>((c, a, t) => c.IsGrounded && !c.ChainThrowCombat.ChainThrusting);
        }
    }
}