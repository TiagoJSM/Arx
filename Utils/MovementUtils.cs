using CommonInterfaces.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public static class MovementUtils
    {
        public static Direction DirectionOfMovement(float horizontal, Direction defaultValue)
        {
            if (horizontal > 0)
            {
                return Direction.Right;
            }
            else if (horizontal < 0)
            {
                return Direction.Left;
            }
            return defaultValue;
        }
    }
}
