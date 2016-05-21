using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Waypoint
{
    public enum StartLocation
    {
        CurrentPosition,
        Start,
        End
    }

    public class WaypointPathController : MonoBehaviour
    {
        private Vector2[] _pathNodes;

        private int _pathIdx;
        private Vector2 _overflow;

        public StartLocation startLocation = StartLocation.CurrentPosition;
        public float waypointThreasholdRadius = 1.2f;
        public float velocity = 1;
        public WaypointPath waypointPath;

        // Use this for initialization
        void Start()
        {
            _pathNodes = waypointPath.PathNodes.ToArray();
            if (startLocation == StartLocation.Start)
            {
                this.transform.position = GetWaypoint().ToVector3();
            }
            else if (startLocation == StartLocation.End)
            {
                _pathIdx = _pathNodes.Length - 1;
                this.transform.position = GetWaypoint().ToVector3();
            }
        }

        void FixedUpdate()
        {
            UpdateWaypointIndex();
            var waypoint = GetWaypoint();
            var normalized = (waypoint - transform.position.ToVector2()).normalized;
            var position = transform.position.ToVector2() + normalized * velocity * Time.fixedDeltaTime;
            transform.position = position - _overflow;
            _overflow = Vector2.zero;

            var distance =
                Vector3
                    .Distance(
                        transform.position,
                        waypoint.ToVector3());
            if (distance <= waypointThreasholdRadius)
            {
                _overflow = transform.position.ToVector2() - waypoint;
            }
        }

        private Vector2 GetWaypoint()
        {
            return _pathNodes[_pathIdx];
        }

        private void UpdateWaypointIndex()
        {
            var waypoint = GetWaypoint();
            var distance =
                Vector3
                    .Distance(
                        transform.position,
                        waypoint.ToVector3());

            if (distance <= waypointThreasholdRadius)
            {
                _pathIdx++;
            }

            if (_pathIdx >= _pathNodes.Length)
            {
                _pathIdx = 0;
            }
        }
    }
}
