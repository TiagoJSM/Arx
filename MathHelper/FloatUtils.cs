using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MathHelper
{
    public static class FloatUtils
    {
        public const float FullDegreeTurn = 360;
        public const float HalfDegreeTurn = FullDegreeTurn / 2;

        public static bool IsApproximately(float a, float b, float tolerance)
        {
            return Mathf.Abs(a - b) < tolerance;
        }

        public static float AngleBetween(Vector3 origin, Vector3 other)
        {
            var dir = other - origin;
            return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }

        public static Vector2 ClosestPointOnLine(Vector2 v1, Vector2 v2, Vector2 point)
        {
            var vVector1 = point - v1;
            var vVector2 = (v2 - v1).normalized;

            var d = Vector2.Distance(v1, v2);
            var t = Vector2.Dot(vVector2, vVector1);

            if (t <= 0)
                return v1;

            if (t >= d)
                return v2;

            var vVector3 = vVector2 * t;

            var closestPoint = v1 + vVector3;

            return closestPoint;
        }
    }
}
