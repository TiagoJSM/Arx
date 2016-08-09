using CommonInterfaces.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions
{
    public static class DirectionExtensions
    {
        public static  float DirectionValue(this Direction defaultValue)
        {
            return defaultValue == Direction.Left ? -1 : 1;
        }
    }
}
