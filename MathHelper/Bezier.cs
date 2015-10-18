using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MathHelper
{
    //Source: http://devmag.org.za/2011/04/05/bzier-curves-a-tutorial/
    public static class Bezier
    {
        public static Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            var u = 1.0f - t;
            var tt = t * t;
            var uu = u * u;
            var uuu = uu * u;
            var ttt = tt * t;

            var p = uuu * p0;       //first term
            p += 3 * uu * t * p1;   //second term
            p += 3 * u * tt * p2;   //third term
            p += ttt * p3;          //fourth term
 
            return p;
        }
    }
}
