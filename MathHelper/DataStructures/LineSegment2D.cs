using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MathHelper.DataStructures
{
    public struct LineSegment2D
    {
        public Vector2 P1;
        public Vector2 P2;

        public Vector2 NormalVector
        {
            get
            {
                return new Vector2(-(P2.y - P1.y), (P2.x - P1.x));
            }
        }

        public LineSegment2D(Vector2 p1, Vector2 p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public float? Slope
        {
            get
            {
                var xDif = P2.x - P1.x;
                if (xDif == 0)
                {
                    return null;
                }
                return (P2.y - P1.y) / (xDif);
            }
        }

        public static float CalculateY(float x, float b, float m)
        {
            return x * m + b;
        }

        public static float CalculateB(float x, float y, float m)
        {
            return y - (x * m);
        }
    }
}
