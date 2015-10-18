using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MathHelper
{
    public static class LaGrande
    {
        public static float GetValue(Vector2[] controlPoints, float x)
        {
            var result = 0.0f;
            for (var j = 0; j < controlPoints.Length; j++)
            {
                result += Inner(controlPoints, x, j);
            }
            return result;
        }

        private static float Inner(Vector2[] controlPoints, float x, int j)
        {
            var dividend = 1.0f;
            var divisor = 1.0f;
            for (var k = 0; k < controlPoints.Length; k++)
            {
                if (k == j)
                {
                    continue;
                }
                dividend *= (x - controlPoints[k].x);
                divisor *= (controlPoints[j].x - controlPoints[k].x);
            }
            return (dividend/divisor)*controlPoints[j].y;
        }
    }
}
