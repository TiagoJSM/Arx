using _2DDynamicCamera.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace _2DDynamicCamera.Zones
{
    public class ZoomZone : BaseZone
    {
        private float _previousCameraZoom;

        public float zoom = 3;

        protected override void OnCameraOwnerEnter()
        {
            DynamicCamera.Main.Zoom(zoom);
        }

        protected override void OnCameraOwnerExit()
        {
            DynamicCamera.Main.ZoomToDefault();
        }
    }
}
