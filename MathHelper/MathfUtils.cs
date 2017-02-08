using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MathHelper
{
    public class Parabola
    {
        public float a;
        public float b;
        public float c;

        public bool HasInvalidValue
        {
            get
            {
                return float.IsNaN(a) || float.IsNaN(b) || float.IsNaN(c);
            }
        }
    }

    public static class MathfUtils
    {
        public static Parabola GetParabola(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            //var b = [(y₀-y₁)(x₁²-x₂²)+(y₂-y₁)(x₀²-x₁²)] ⁄ [(x₀-x₁)(x₁²-x₂²)+(x₂-x₁)(x₀²-x₁²)]
            var b = ((p0.y-p1.y) * ((p1.x * p1.x ) - (p2.x * p2.x)) + (p2.y-p1.y) * ((p0.x * p0.x) - (p1.x * p1.x))) / ((p0.x - p1.x) * ((p1.x * p1.x) - (p2.x * p2.x)) + (p2.x - p1.x) * ((p0.x * p0.x) - (p1.x * p1.x)));

            //var a = [y₀-y₁-b⋅(x₀-x₁)] ⁄ (x₀²-x₁²) 
            var a = (p0.y - p1.y + ((-b)*(p0.x - p1.x))) / ((p0.x * p0.x) - (p1.x * p1.x));

            //var c = y₀ -a⋅x₀² -b⋅x₀ 
            var c = p0.y - a * (p0.x * p0.x) - b * p0.x;

            return new Parabola()
            {
                a = a,
                b = b,
                c = c
            };
        }

        public static Vector2 QuadraticInterpolation(float xStart, float xEnd, Parabola parabola, float t)
        {
            var x = Mathf.Lerp(xStart, xEnd, t);
            var y = parabola.a * (x * x) + parabola.b * x + parabola.c;
            return new Vector2(x, y);
        }
    }
}
