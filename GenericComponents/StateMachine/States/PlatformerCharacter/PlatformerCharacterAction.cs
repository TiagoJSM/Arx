using GenericComponents.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericComponents.StateMachine.States.PlatformerCharacter
{
    public class PlatformerCharacterAction
    {
        public float Move { get; private set; }
        public float Vertical { get; private set; }
        public bool Jump { get; private set; }
        public bool Roll { get; private set; }
        public AttackType AttackType { get; private set; }
        public bool GrabRope { get; private set; }

        public PlatformerCharacterAction(
            float move, 
            float vertical, 
            bool jump,
            bool roll,
            AttackType attackType,
            bool grabRope)
        {
            Move = move;
            Vertical = vertical;
            Jump = jump;
            Roll = roll;
            AttackType = attackType;
            GrabRope = grabRope;
        }
    }
}
