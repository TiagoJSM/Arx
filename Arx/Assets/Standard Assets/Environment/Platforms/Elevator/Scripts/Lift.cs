using Assets.Standard_Assets.Common;
using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Platforms.Elevator.Scripts
{
    public enum ElevatorPoints
    {
        Point1,
        Point2
    }
    public class Lift : MonoBehaviour
    {
        private Coroutine _activeRoutine;

        [SerializeField]
        private Transform _point1;
        [SerializeField]
        private Transform _point2;
        [SerializeField]
        private ElevatorPoints _startingPoint;
        [SerializeField]
        private float speed = 20;
        [SerializeField]
        private LayerMask _playerLayer;
        [SerializeField]
        private bool _active = true;

        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
            }
        }

        private void Start()
        {
            transform.position = _startingPoint == ElevatorPoints.Point1 ? _point1.position : _point2.position;
        }

        public void GoToPoint1()
        {
            GoToPoint(_point1, true);
        }

        public void GoToPoint2()
        {
            GoToPoint(_point2, true);
        }

        private void GoToPoint(Transform point, bool discardCurrentLiftMovement = false)
        {
            if (!_active)
            {
                return;
            }

            if (discardCurrentLiftMovement && _activeRoutine != null)
            {
                StopCoroutine(_activeRoutine);
                _activeRoutine = null;
            }
            if (_activeRoutine != null)
            {
                return;
            }

            var distance = Vector3.Distance(transform.position, point.position);
            var elevatorRoutine = CoroutineHelpers.MoveTo(transform.position, point.position, distance / speed, transform, () => _activeRoutine = null);
            _activeRoutine = StartCoroutine(elevatorRoutine);
        }

        private IEnumerator ElevatorRoutine(Transform target)
        {
            var origin = transform.position;
            var distance = Vector3.Distance(transform.position, target.position);
            var time = distance / speed;
            var elapsed = 0.0f;

            while(time > elapsed)
            {
                transform.position = Vector3.Lerp(origin, target.position, elapsed / time);
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.position = target.position;
            _activeRoutine = null;
        }

        private void GoToOtherPoint()
        {
            var otherPoint = Vector3.Distance(transform.position, _point1.position) < Mathf.Epsilon ? _point2 : _point1;
            GoToPoint(otherPoint);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!_active)
            {
                return;
            }

            if(_activeRoutine == null && !collider.isTrigger)
            {
                if (_playerLayer.IsInAnyLayer(collider.gameObject))
                {
                    GoToOtherPoint();
                }
            }
        }
    }
}
