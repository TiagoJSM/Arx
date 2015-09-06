using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DevelopmentUtils
{
    public static class GizmoUtils
    {
        public static void DrawSquare(Vector3 topLeft, Vector3 topRight, Vector3 bottomRight, Vector3 bottomLeft, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }
    }
}
