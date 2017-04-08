using Assets.Standard_Assets._2D.Scripts.EnvironmentDetection;
using Assets.Standard_Assets.GenericComponents.Waypoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Treadmill.Scripts
{
    [RequireComponent(typeof(Pushable))]
    [RequireComponent(typeof(WaypointPathController))]
    public class TreadmillCrate : MonoBehaviour
    {
        private Pushable _pushable;
        private WaypointPathController _waypointPath;

        private void Awake()
        {
            _pushable = GetComponent<Pushable>();
            _waypointPath = GetComponent<WaypointPathController>();
        }

        public void MoveOnPath()
        {
            _pushable.enabled = false;
            _waypointPath.enabled = true;
        }
    }
}
