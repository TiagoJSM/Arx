using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;

namespace _2DDynamicCamera
{
    public class DynamicCamera : MonoBehaviour
    {
        public Transform target;
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
        [Range(0, float.MaxValue)]
        public float zoom = 3.3f;

        private float _currentXVelocity;
        private float _currentYVelocity;
        private float _offsetZ;

        // Use this for initialization
        private void Start()
        {
            _offsetZ = (transform.position - target.position).z;
            if (startOnTarget)
            {
                var position = target.position;
                position.z = _offsetZ;
                transform.position = position;
            }
            //transform.parent = null;
        }

        // Update is called once per frame
        private void Update()
        {
            Vector3 targetPosition = target.position;
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

            cameraPosition.z = _offsetZ;

            transform.position = cameraPosition;

            Camera.main.orthographicSize = zoom;
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
