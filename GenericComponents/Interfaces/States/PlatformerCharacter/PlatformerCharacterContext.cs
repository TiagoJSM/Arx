using CommonInterfaces.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Interfaces.States.PlatformerCharacter
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
        WeaponType? WeaponType { get;}
        bool SlidingDown { get; }
        IEnumerable<RaycastHit2D> FrameHits { get; }
        bool IsCharging { get; }

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
    }
}
