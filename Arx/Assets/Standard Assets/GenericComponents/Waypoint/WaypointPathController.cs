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
        private float _elapsedTime;
        private float _timeToPoint;
        private Vector3 _origin;
        private Vector3 _target;
        private float _waitTime;

        [SerializeField]
        private bool _restartWhenFinish;
        [SerializeField]
        private int _initialWaypointTarget;
        [SerializeField]
        private UpdateMode _updateMode = UpdateMode.Update;
        [SerializeField]
        private float _waitTimeBetweenPoints;

        public StartLocation startLocation = StartLocation.CurrentPosition;
        //public float waypointThreasholdRadius = 1.2f;
        public float velocity = 1;
        public WaypointPath waypointPath;

        // Use this for initialization
        void Start()
        {
            //PathNodes = waypointPath.PathNodes.ToArray();
            if (startLocation == StartLocation.Start)
            {
                this.transform.position = GetWaypoint().ToVector3();
                _pathIdx = 1;
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
            UpdateSegmentData();
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
            if(_waitTime > 0)
            {
                _waitTime -= delta;
                return;
            }
            UpdateWaypointIndex();
            transform.position = Vector3.Lerp(_origin, _target, _elapsedTime / _timeToPoint);
            _elapsedTime += delta;

        }

        private Vector2 GetWaypoint()
        {
            return PathNodes[_pathIdx];
        }

        private void UpdateWaypointIndex()
        {
            var updateSegmentData = false;
            if (_elapsedTime >= _timeToPoint)
            {
                _pathIdx++;
                updateSegmentData = true;
                _waitTime = _waitTimeBetweenPoints;
            }

            if (_pathIdx >= PathNodes.Length)
            {
                _pathIdx = 0;
                updateSegmentData = true;

                if (_pathIdx == 0 && _restartWhenFinish)
                {
                    transform.position = PathNodes.First();
                    _pathIdx++;
                }
            }

            if (updateSegmentData)
            {
                UpdateSegmentData();
            }
        }

        private void UpdateSegmentData()
        {
            _elapsedTime = 0;
            _timeToPoint = GetTimeToPoint();
            _origin = transform.position;
            _target = GetWaypoint();
        }

        private float GetTimeToPoint()
        {
            var waypoint = GetWaypoint();
            var distance =
                Vector3
                    .Distance(
                        transform.position,
                        waypoint.ToVector3());

            return distance / velocity;
        }
    }
}
