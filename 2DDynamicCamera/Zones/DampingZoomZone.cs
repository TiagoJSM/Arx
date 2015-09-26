using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace _2DDynamicCamera.Zones
{
    public class DampingZoomZone : BaseZone
    {
        private float _previousCameraZoom;

        public float zoom = 3;
        [Range(0, 1)]
        public float damping = 2f;

        protected override void OnCameraOwnerEnter()
        {
            DynamicCamera.Main.Zoom(zoom, damping);
        }

        protected override void OnCameraOwnerExit()
        {
            DynamicCamera.Main.ZoomToDefault(damping);
        }
    }
}
