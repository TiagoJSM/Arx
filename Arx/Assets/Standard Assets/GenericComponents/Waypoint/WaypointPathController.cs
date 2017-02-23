using Extensions;
using GenericComponents.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.GenericComponents.Waypoint
{
    public enum StartLocation
    {
        CurrentPosition,
        Start,
        End
    }

    public class WaypointPathController : MonoBehaviour
    {
        private Vector2[] PathNodes
        {
            get
            {
                return waypointPath.PathNodes.ToArray();
            }
        }

        private int _pathIdx;
        private Vector2 _overflow;

        [SerializeField]
        private bool _restartWhenFinish;
        [SerializeField]
        private int _initialWaypointTarget;
        [SerializeField]
        private UpdateMode _updateMode = UpdateMode.Update;

        public StartLocation startLocation = StartLocation.CurrentPosition;
        public float waypointThreasholdRadius = 1.2f;
        public float velocity = 1;
        public WaypointPath waypointPath;

        // Use this for initialization
        void Start()
        {
            //PathNodes = waypointPath.PathNodes.ToArray();
            if (startLocation == StartLocation.Start)
            {
                this.transform.position = GetWaypoint().ToVector3();
            }
            else if (startLocation == StartLocation.End)
            {
                _pathIdx = PathNodes.Length - 1;
                this.transform.position = GetWaypoint().ToVector3();
            }
            else if (startLocation == StartLocation.CurrentPosition)
            {
                //check if _initialWaypointTarget is out of bounds
                _pathIdx = Math.Min(PathNodes.Length - 1, _initialWaypointTarget);
            }
        }

        void FixedUpdate()
        {
            if (_updateMode == UpdateMode.FixedUpdate)
            {
                UpdateObject(Time.fixedDeltaTime);
            }
        }

        void Update()
        {
            if (_updateMode == UpdateMode.Update)
            {
                UpdateObject(Time.deltaTime);
            }
        }

        private void UpdateObject(float delta)
        {
            UpdateWaypointIndex();
            if (_pathIdx == 0 && _restartWhenFinish)
            {
                transform.position = PathNodes.First();
                _pathIdx++;
            }
            var waypoint = GetWaypoint();
            var normalized = (waypoint - transform.position.ToVector2()).normalized;
            var position = transform.position.ToVector2() + normalized * velocity * delta;
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
            return PathNodes[_pathIdx];
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

            if (_pathIdx >= PathNodes.Length)
            {
                _pathIdx = 0;
            }
        }
    }
}
