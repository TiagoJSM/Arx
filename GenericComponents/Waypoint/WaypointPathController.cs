using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Waypoint
{
    public class WaypointPathController : MonoBehaviour
    {
        private Vector2[] _pathNodes;

        private int _pathIdx;

        public bool startAtWaypoint = true;
        public float waypointThreasholdRadius = 1.2f;
        public float velocity = 1;
        public WaypointPath waypointPath;

        // Use this for initialization
        void Start()
        {
            _pathNodes = waypointPath.PathNodes.ToArray();
            if (startAtWaypoint)
            {
                this.transform.position = GetWaypoint().ToVector3();
            }
        }

        void FixedUpdate()
        {
            UpdateWaypointIndex();
            var normalized = (GetWaypoint() - transform.position.ToVector2()).normalized;
            var position = transform.position.ToVector2() + normalized * velocity * Time.deltaTime;
            transform.position = position;
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
