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

        void DoMove(float move);
        void DoGrabLedge();
        void DropLedge();
        void JumpUp();
        void Duck();
        void Stand();
        void StayStill();
    }
}
