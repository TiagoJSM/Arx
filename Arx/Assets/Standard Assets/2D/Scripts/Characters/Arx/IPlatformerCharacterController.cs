using ArxGame.Components.Environment;
using Assets.Standard_Assets._2D.Scripts.EnvironmentDetection;
using CommonInterfaces.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx
{
    public interface IPlatformerCharacterController
    {
        bool IsGrounded { get; }
        bool CanGrabLedge { get; }
        float VerticalSpeed { get; }
        bool GrabbingLedge { get; }
        bool CanStand { get; }
        int ComboNumber { get; }
        WeaponType? WeaponType { get; }
        bool SlidingDown { get; }
        IEnumerable<RaycastHit2D> FrameHits { get; }
        bool IsCharging { get; }
        bool RopeFound { get; }
        GrappleRope GrappleRope { get; }
        Pushable Pushable { get; }
        Vector3? SafeSpot { get; }
        bool AttackedThisFrame { get; }
        bool CanBeAttacked { get; set; }
        bool Attacking { get; }
        bool LadderFound { get; }

        void DoMove(float move);
        void DoGrabLedge();
        void DropLedge();
        void JumpUp(float jumpRatio);
        void Duck();
        void Stand();
        void StayStill();
        void Roll(float move);
        void StartIddle();
        void StopIddle();
        void FlipToSlideDownDirection();
        void StopAirSlash();
        void DoChargeAttack();
        void DoReleaseChargeAttack();
        void GrabRope();
        void LetGoRope();
        void MoveOnRope(float horizontal, float vertical);
        void DoAimingMove(float move);
        void SetDirectionToAimDirection();
        void DoShoot();
        void Aim(bool aim);
        void DoThrow();
        void GrabGrapple();
        void MoveOnGrapple(float horizontal, float vertical);
        void ReleaseGrapple();
        void PushObject();
        void StartMovingToSafeSpot();
        void StopMovingToSafeSpot();
        void LaunchCharacter();
        void AttackStateDone();
        void GrabLadder();
        void MoveOnLadder(float vertical);
        void LetGoLadder();
        void StartFlashing();
        void StopFlashing();
    }
}
