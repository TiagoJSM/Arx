﻿using ArxGame.Components.Environment;
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
        bool IsAttackOver { get; }
        int ComboNumber { get; }
        WeaponType? WeaponType { get; }
        bool SlidingDown { get; }
        IEnumerable<RaycastHit2D> FrameHits { get; }
        bool IsCharging { get; }
        bool RopeFound { get; }
        GrappleRope GrappleRope { get; }
        Pushable Pushable { get; }
        Vector3? SafeSpot { get; }

        void DoMove(float move);
        void DoGrabLedge();
        void DropLedge();
        void JumpUp();
        void Duck();
        void Stand();
        void StayStill();
        void Roll(float move);
        void DoPrimaryGroundAttack();
        void DoPrimaryAirAttack();
        void DoSecundaryGroundAttack();
        void DoSecundaryAirAttack();
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
        void Push();
        void StartMovingToSafeSpot();
        void StopMovingToSafeSpot();
    }
}