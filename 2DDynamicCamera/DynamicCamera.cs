using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using _2DDynamicCamera.Interfaces;
using MathHelper;
using System.Collections;

namespace _2DDynamicCamera
{
    public enum UpdateMode
    {
        FixedUpdate,
        LateUpdate
    }

    [RequireComponent(typeof(Camera))]
    public class DynamicCamera : MonoBehaviour
    {
        private Camera _camera;
        private IEnumerator _zoomCoroutine;
        private bool _targetTransition;

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
        public UpdateMode updateMode;

        [SerializeField]
        private Transform _owner;
        [Range(0, 100)]
        [SerializeField]
        private float _defaultZoom = 3.3f;
        private float _zoomThreshold = 0.03f;

        private float _currentXVelocity;
        private float _currentYVelocity;
        private float _cachedOffsetZ;
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
            _targetTransition = true;
        }

        public void RemoveTarget(ICameraTarget target)
        {
            _targets.Remove(target);
            _targetTransition = true;
        }

        public void SetDefaultZoom(float zoom)
        {
            _defaultZoom = zoom;
        }

        public void Zoom(float zoom)
        {
            StopZoomCoroutine();
            _camera.orthographicSize = zoom;

        }

        public void Zoom(float zoom, float damping)
        {
            StartZoomCoroutine(damping, zoom);
        }

        public void ZoomToDefault()
        {
            StopZoomCoroutine();
            _camera.orthographicSize = _defaultZoom;
        }

        public void ZoomToDefault(float damping)
        {
            StartZoomCoroutine(damping, _defaultZoom);
        }

        // Use this for initialization
        private void Start()
        {
            _targets = new List<ICameraTarget>();
            _cachedOffsetZ = OffsetZ;
            _camera = GetComponent<Camera>();
            if (startOnTarget)
            {
                var position = _owner.position;
                position.z = _cachedOffsetZ;
                transform.position = position;
            }
        }

        private void FixedUpdate()
        {
            if (updateMode == UpdateMode.FixedUpdate)
            {
                CameraUpdate();
            }
        }

        private void LateUpdate()
        {
            if(updateMode == UpdateMode.LateUpdate)
            {
                CameraUpdate();
            }
        }

        public IEnumerator ZoomOverTime(float damping, float targetZoom)
        {
            var currentZoom = _camera.orthographicSize;
            var currentZoomVelocity = 0.0f;

            while (true)
            {
                if (FloatUtils.IsApproximately(currentZoom, targetZoom, _zoomThreshold))
                {
                    _camera.orthographicSize = targetZoom;
                    yield break;
                }

                currentZoom = Mathf.SmoothDamp(currentZoom, targetZoom, ref currentZoomVelocity, damping);
                _camera.orthographicSize = currentZoom;

                yield return null;
            }
        }

        private void CameraUpdate()
        {
            Vector3 targetPosition = TargetPosition;
            var cameraPositionRelative = transform.position - targetPosition;
            bool updateNeeded = Vector3.Distance(Vector3.zero, cameraPositionRelative) > targetDistanceThreshold;

            if (!updateNeeded)
            {
                _targetTransition = false;
                return;
            }

            var x = Mathf.SmoothDamp(cameraPositionRelative.x, 0, ref _currentXVelocity, xDamping);
            var y = Mathf.SmoothDamp(cameraPositionRelative.y, 0, ref _currentYVelocity, yDamping);

            if (!_targetTransition)
            {
                x = Mathf.Clamp(x, -xBounds, xBounds);
                y = Mathf.Clamp(y, -yBounds, yBounds);
            }

            cameraPositionRelative.x = x;
            cameraPositionRelative.y = y;

            var cameraPosition = cameraPositionRelative + targetPosition;

            cameraPosition.z = _cachedOffsetZ;

            transform.position = cameraPosition;
        }

        private void StartZoomCoroutine(float damping, float targetZoom)
        {
            StopZoomCoroutine();
            _zoomCoroutine = ZoomOverTime(damping, targetZoom);
            StartCoroutine(_zoomCoroutine);
        }

        private void StopZoomCoroutine()
        {
            if (_zoomCoroutine != null)
            {
                StopCoroutine(_zoomCoroutine);
            }
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
