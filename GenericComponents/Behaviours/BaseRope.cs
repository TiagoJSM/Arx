using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Behaviours
{
    public abstract class BaseRope : MonoBehaviour
    {
        public abstract Vector2[] Points { get; }
        public float RopeSize
        {
            get
            {
                var ropePoints = Points;
                if (ropePoints.Length < 2)
                {
                    return 0;
                }
                var size = 0f;
                var previousPoint = ropePoints[0];
                for (var idx = 1; idx < ropePoints.Length; idx++)
                {
                    size += Vector2.Distance(previousPoint, ropePoints[idx]);
                    previousPoint = ropePoints[idx];
                }
                return size;
            }
        }
    }
}
