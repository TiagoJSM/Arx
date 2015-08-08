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

        public float B
        {
            get
            {
                //y = m.x + b
                //b = y - m.x
                return P1.y - (Slope.Value * P1.x);
            }
        }

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

        public float Lenght
        {
            get
            {
                return Mathf.Abs(Vector2.Distance(P1, P2));
            }
        }

        public float XWhenYIs(float y)
        {
            //y = m.x + b
            //x = (y - b) / m
            return (y - B) / Slope.Value;
        }

        public override string ToString()
        {
            return "(" + P1.ToString() + ";" + P2.ToString() + ")";
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
