using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MathHelper.DataStructures
{
    public struct BezierLineSegment2D
    {
        public LineSegment2D LineSegment;

        public Vector2 P1ControlPoint;
        public Vector2 P2ControlPoint;

        public BezierLineSegment2D(LineSegment2D lineSegment)
        {
            LineSegment = lineSegment;
            P1ControlPoint = lineSegment.P1;
            P2ControlPoint = lineSegment.P2;
        }

        public BezierLineSegment2D(LineSegment2D lineSegment, Vector2 p1ControlPoint, Vector2 p2ControlPoint)
        {
            LineSegment = lineSegment;
            P1ControlPoint = p1ControlPoint;
            P2ControlPoint = p2ControlPoint;
        }

        public override string ToString()
        {
            return "(" + LineSegment.P1.ToString() + ";" + LineSegment.P2.ToString() + ":" + P1ControlPoint.ToString() + ";" + P2ControlPoint.ToString() + ")";
        }
    }
}
