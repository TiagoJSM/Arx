using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Utils
{
    public static class CameraUtils
    {
        public static void SetActiveCamera(Camera cam)
        {
            var cameras = Camera.allCameras;
            for(var idx = 0; idx < cameras.Length; idx++)
            {
                var camera = cameras[idx];
                camera.enabled = false;
            }
            cam.enabled = true;
        }
    }
}
