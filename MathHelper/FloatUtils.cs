using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MathHelper
{
    public static class FloatUtils
    {
        public static bool IsApproximately(float a, float b, float tolerance)
        {
            return Mathf.Abs(a - b) < tolerance;
        }
    }
}
