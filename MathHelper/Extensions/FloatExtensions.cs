using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MathHelper.Extensions
{
    public static class FloatExtensions
    {
        public static float NormalizeRadians(this float radians)
        {
            radians = radians % (2* Mathf.PI);
            if (radians < 0)
            {
                radians += (2 * Mathf.PI);
            }
            return radians;
        }

        public static int GetQuadrant(this float radians)
        {
            radians = radians.NormalizeRadians();
            if (radians >= 0 && radians < Mathf.PI / 2)
            {
                return 0;
            }
            else if (radians >= (Mathf.PI / 2) && radians < Mathf.PI)
            {
                return 1;
            }
            else if (radians >= Mathf.PI && radians < (3 * Mathf.PI / 2))
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }

        public static float ReduceToQuadrant0and3(this float radians)
        {
            radians = radians.NormalizeRadians();
            var quadrant = radians.GetQuadrant();
            if (quadrant == 0 || quadrant == 3)
            {
                return radians;
            }

            if (quadrant == 1)
            {
                var difference = radians - (Mathf.PI / 2);
                return radians - (difference * 2);
            }
            var diff = (3 * Mathf.PI / 2) - radians;
            return radians + (diff * 2);
        }

        public static bool IsInNegativeXQuadrant(this float radians)
        {
            radians = radians.NormalizeRadians();
            if(radians > (Mathf.PI / 2) && radians < (3 * Mathf.PI / 2))
            {
                return true;
            }
            return false;
        }

        public static bool IsInPositiveXQuadrant(this float radians)
        {
            radians = radians.NormalizeRadians();
            if (radians < (Mathf.PI / 2) || radians > (3 * Mathf.PI / 2))
            {
                return true;
            }
            return false;
        }

        public static float ReduceToSingleTurn(this float degrees)
        {
            var turns = (int)(degrees / FloatUtils.FullDegreeTurn);

            if(degrees < 0)
            {
                turns = -turns;
                var reduced = degrees + (turns * FloatUtils.FullDegreeTurn);
                return reduced + FloatUtils.FullDegreeTurn;
            }
            return degrees - (turns * FloatUtils.FullDegreeTurn);
        }

        public static Vector3 GetDirectionVectorFromDegreeAngle(this float degrees)
        {
            var angleInRadians = degrees * Mathf.Deg2Rad;
            return new Vector3(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));
        }
    }
}
