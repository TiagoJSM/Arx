using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets
{
    public static class MathUtilities
    {
        public static Vector2 ExponentialInterpolation(Vector2 origin, Vector2 target, float duration, ref float elapsedLerpTime)
        {
            float t = elapsedLerpTime / duration;
            t = t * t;
            elapsedLerpTime += Time.deltaTime;
            return Vector2.Lerp(origin, target, t);
        }

        public static float ExponentialInterpolation(float origin, float target, float duration, ref float elapsedLerpTime)
        {
            float t = elapsedLerpTime / duration;
            t = t * t;
            elapsedLerpTime += Time.deltaTime;
            return Mathf.Lerp(origin, target, t);
        }
    }
}
