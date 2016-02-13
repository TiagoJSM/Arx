using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using _2DDynamicCamera.Interfaces;
using MathHelper;

namespace _2DDynamicCamera
{
    public class DynamicCamera : MonoBehaviour
    {
        [Range(0, float.MaxValue)]
        public float xDamping = 0.1f;
        [Range(0, float.MaxValue)]
        public float yDamping = 0.1f;
        public float targetDistanceThreshold = 0.03f;
        public bool startOnTarget = true;
        [Range(0, float.MaxValue)]
        public float xBounds;
        [Range(0, float.MaxValue)]
        public float yBounds;
        public Vector2 offset;

        [SerializeField]
        private Transform _owner;
        [Range(0, 100)]
        [SerializeField]
        private float _defaultZoom = 3.3f;
        private float _zoomThreshold = 0.03f;

        private float _currentXVelocity;
        private float _currentYVelocity;
        private float _zoomTarget;
        private float _cachedOffsetZ;
        private float _currentZoom;
        private float _zoomDamping;
        private List<ICameraTarget> _targets;

        private Vector3 TargetPosition
        {
            get
            {
                Vector3 targetPosition;
                if (_targets.Any())
                {
                    targetPosition = _targets.Last().Position;
                }
                else if(_owner != null)
                {
                    targetPosition = _owner.position;
                }
                else
                {
                    targetPosition = transform.position;
                }
                return targetPosition + offset.ToVector3();
            }
        }

        private float OffsetZ
        {
            get
            {
                if(_owner == null)
                {
                    return 0;
                }
                return (transform.position - _owner.position).z;
            }
        }

        public Transform Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
                _cachedOffsetZ = OffsetZ;
            }
        }

        public static DynamicCamera Main
        {
            get
            {
                return Camera.main.GetComponent<DynamicCamera>();
            }
        }

        public void AddTarget(ICameraTarget target)
        {
            _targets.Add(target);
        }

        public void RemoveTarget(ICameraTarget target)
        {
            _targets.Remove(target);
        }

        public void SetDefaultZoom(float zoom)
        {
            _defaultZoom = zoom;
        }

        public void Zoom(float zoom)
        {
            _currentZoom = zoom;
            _zoomTarget = zoom;
        }

        public void Zoom(float zoom, float damping)
        {
            _zoomTarget = zoom;
            _zoomDamping = damping;
        }

        public void ZoomToDefault()
        {
            _zoomTarget = _defaultZoom;
            _currentZoom = _defaultZoom;
        }

        public void ZoomToDefault(float damping)
        {
            _zoomTarget = _defaultZoom;
            _zoomDamping = damping;
        }

        // Use this for initialization
        private void Start()
        {
            _targets = new List<ICameraTarget>();
            _currentZoom = _defaultZoom;
            _zoomTarget = _defaultZoom;
            _cachedOffsetZ = OffsetZ;
            if (startOnTarget)
            {
                var position = _owner.position;
                position.z = _cachedOffsetZ;
                transform.position = position;
            }
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            Vector3 targetPosition = TargetPosition;
            var cameraPositionRelative = transform.position - targetPosition;
            bool updateNeeded = Vector3.Distance(Vector3.zero, cameraPositionRelative) > targetDistanceThreshold;

            if (!updateNeeded)
            {
                return;
            }

            var x = Mathf.SmoothDamp(cameraPositionRelative.x, 0, ref _currentXVelocity, xDamping);
            var y = Mathf.SmoothDamp(cameraPositionRelative.y, 0, ref _currentYVelocity, yDamping);

            x = Mathf.Clamp(x, -xBounds, xBounds);
            y = Mathf.Clamp(y, -yBounds, yBounds);

            cameraPositionRelative.x = x;
            cameraPositionRelative.y = y;

            var cameraPosition = cameraPositionRelative + targetPosition;

            cameraPosition.z = _cachedOffsetZ;

            transform.position = cameraPosition;

            HandleZoom();
        }

        private void HandleZoom()
        {
            if (FloatUtils.IsApproximately(_currentZoom, _zoomTarget, _zoomThreshold))
            {
                Camera.main.orthographicSize = _currentZoom;
                return;
            }

            float currentVelocity = 0;
            _currentZoom = Mathf.SmoothDamp(_currentZoom, _zoomTarget, ref currentVelocity, _zoomDamping);
            Camera.main.orthographicSize = _currentZoom;
        }

        void OnDrawGizmos()
        {
            DrawCameraBounds();
        }

        private void DrawCameraBounds()
        {
            var cameraPosition = transform.position;
            Gizmos.color = Color.green;
            //Top line
            Gizmos.DrawLine(cameraPosition - new Vector3(-xBounds, yBounds, 0), cameraPosition - new Vector3(xBounds, yBounds, 0));
            //Bottom line
            Gizmos.DrawLine(cameraPosition - new Vector3(-xBounds, -yBounds, 0), cameraPosition - new Vector3(xBounds, -yBounds, 0));
            //Left line
            Gizmos.DrawLine(cameraPosition - new Vector3(-xBounds, yBounds, 0), cameraPosition - new Vector3(-xBounds, -yBounds, 0));
            //Right line
            Gizmos.DrawLine(cameraPosition - new Vector3(xBounds, yBounds, 0), cameraPosition - new Vector3(xBounds, -yBounds, 0));
        }
    }
}
