using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericComponents.Interfaces.States.PlatformerCharacter
{
    public interface IPlatformerCharacterController
    {
        bool IsGrounded { get; }
        bool CanGrabLedge { get; }
        float VerticalSpeed { get; }
        bool GrabbingLedge { get; }
        bool CanStand { get; }
        bool IsCurrentAnimationOver { get; }
        int ComboNumber { get; }

        void DoMove(float move);
        void DoGrabLedge();
        void DropLedge();
        void JumpUp();
        void Duck();
        void Stand();
        void StayStill();
        void Roll(float move);
        void DoLightAttack();
        void DoStrongAttack();
        void AttackIsOver();
    }
}
