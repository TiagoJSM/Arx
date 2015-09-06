using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using Extensions;

namespace DevelopmentUtils
{
    public static class DrawArrow
    {
        public static void ForGizmo(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Gizmos.DrawRay(pos, direction);

            var arrowEnd = pos + direction;

            var right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            var left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(arrowEnd, right * arrowHeadLength);
            Gizmos.DrawRay(arrowEnd, left * arrowHeadLength);
        }

        public static void ForGizmo(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Gizmos.color = color;
            ForGizmo(pos, direction, arrowHeadLength, arrowHeadAngle);
        }

        public static void ForGizmo(Vector2 pos, Vector2 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            var pos3 = pos.ToVector3();
            var direction3 = direction.ToVector3();
            Gizmos.DrawRay(pos, direction);

            var arrowEnd = pos + direction;

            var right = Quaternion.Euler(0, 0, 180 + arrowHeadAngle) * Quaternion.LookRotation(direction) * new Vector3(0, 0, 1);
            var left = Quaternion.Euler(0, 0, 180 - arrowHeadAngle) * Quaternion.LookRotation(direction) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(arrowEnd, right * arrowHeadLength);
            Gizmos.DrawRay(arrowEnd, left * arrowHeadLength);
        }

        public static void ForGizmo(Vector2 pos, Vector2 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Gizmos.color = color;
            ForGizmo(pos, direction, arrowHeadLength, arrowHeadAngle);
        }

        public static void ForDebug(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Debug.DrawRay(pos, direction);
            
            var right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            var left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Debug.DrawRay(pos + direction, right * arrowHeadLength);
            Debug.DrawRay(pos + direction, left * arrowHeadLength);
        }
        public static void ForDebug(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Debug.DrawRay(pos, direction, color);

            var right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            var left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Debug.DrawRay(pos + direction, right * arrowHeadLength, color);
            Debug.DrawRay(pos + direction, left * arrowHeadLength, color);
        }
    }
}
