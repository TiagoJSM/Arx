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
        public AttackType AttackType { get; private set; }

        public PlatformerCharacterAction(
            float move, 
            float vertical, 
            bool jump,
            AttackType attackType)
        {
            Move = move;
            Vertical = vertical;
            Jump = jump;
            AttackType = attackType;
        }
    }
}
