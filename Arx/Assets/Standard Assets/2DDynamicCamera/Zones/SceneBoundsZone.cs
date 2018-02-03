using DevelopmentUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2DDynamicCamera.Zones
{
    public class SceneBoundsZone : MonoBehaviour
    {
        public float topLimit = 10;
        public float bottomLimit = -10;
        public float leftLimit = -10;
        public float rightLimit = 10;

        private Vector3 TopLeft
        {
            get
            {
                return new Vector3(leftLimit, topLimit, 0);
            }
        }

        private Vector3 BottomLeft
        {
            get
            {
                return new Vector3(leftLimit, bottomLimit, 0);
            }
        }

        private Vector3 TopRight
        {
            get
            {
                return new Vector3(rightLimit, topLimit, 0);
            }
        }

        private Vector3 BottomRight
        {
            get
            {
                return new Vector3(rightLimit, bottomLimit, 0);
            }
        }

        public void Contain(DynamicCamera camera)
        {
            var cameraPosition = camera.transform.position;
            cameraPosition.x = Mathf.Clamp(cameraPosition.x, leftLimit, rightLimit);
            cameraPosition.y = Mathf.Clamp(cameraPosition.y, bottomLimit, topLimit);
            camera.transform.position = cameraPosition;
        }

        void Start()
        {
            Camera.onPreRender += AdjustCamera;
        }

        void OnDestroy()
        {
            Camera.onPreRender -= AdjustCamera;
        }

        private void AdjustCamera(Camera cam)
        {
            var cameraPosition = cam.transform.position;
            cameraPosition.x = Mathf.Clamp(cameraPosition.x, leftLimit, rightLimit);
            cameraPosition.y = Mathf.Clamp(cameraPosition.y, bottomLimit, topLimit);
            cam.transform.position = cameraPosition;
        }

        void OnDrawGizmos()
        {
            DrawBounds();
        }

        private void DrawBounds()
        {
            GizmoUtils.DrawSquare(TopLeft, TopRight, BottomRight, BottomLeft, Color.red);
        }
    }
}
