using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Extensions
{
    public static class CameraExtensions
    {
        public static Bounds OrthographicBounds(this Camera camera)
        {
            var screenAspect = (float)Screen.width / (float)Screen.height;
            var cameraHeight = camera.orthographicSize * 2;
            var bounds = new Bounds(
                camera.transform.position,
                new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
            return bounds;
        }
    }
}
