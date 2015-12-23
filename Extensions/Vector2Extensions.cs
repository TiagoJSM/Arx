using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Extensions
{
    public static class Vector2Extensions
    {
        public static Vector3 ToVector3(this Vector2 vec2)
        {
            return new Vector3(vec2.x, vec2.y);
        }

        public static IEnumerable<Vector3> ToVector3s(this IEnumerable<Vector2> vec2s)
        {
            return vec2s.Select(v => v.ToVector3());
        }

        public static Vector2 RotateAround(this Vector2 point, Vector2 center, float angleInRadians)
        {
            float s = Mathf.Sin(angleInRadians);
            float c = Mathf.Cos(angleInRadians);

            // translate point back to origin:
            point.x -= center.x;
            point.y -= center.y;

            // rotate point
            float xnew = point.x * c - point.y * s;
            float ynew = point.x * s + point.y * c;

            // translate point back:
            point.x = xnew + center.x;
            point.y = ynew + center.y;
            return point;
        }

        public static bool IsInRadius(this Vector2 center, Vector2 point, float radius)
        {
            return Mathf.Abs(Vector2.Distance(center, point)) < radius;
        }
    }
}
