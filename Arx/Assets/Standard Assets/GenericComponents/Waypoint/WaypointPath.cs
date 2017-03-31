using Assets.Standard_Assets.GenericComponents.Behaviours;
using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.GenericComponents.Waypoint
{
    public class WaypointPath : NodePathBehaviour
    {
        private void OnDrawGizmos()
        {
            foreach (var bezierLineSegment in InScenePathSegments)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(bezierLineSegment.P1.ToVector3(), bezierLineSegment.P2.ToVector3());
                //Handles.color = EndColor;
                //Gizmos.DrawLine(bezierLineSegment.P2.ToVector3(), bezierLineSegment.P2ControlPoint.ToVector3());
            }
        }
    }
}
