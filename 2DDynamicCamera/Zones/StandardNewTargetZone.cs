using _2DDynamicCamera.Zones.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace _2DDynamicCamera.Zones
{
    public enum ColliderType
    {
        Box, 
        Circle
    }

    public class StandardNewTargetZone : NewTargetZone
    {
        protected override void OnCameraOwnerEnter()
        {
            DynamicCamera.Main.AddTarget(this);
        }

        protected override void OnCameraOwnerExit()
        {
            DynamicCamera.Main.RemoveTarget(this);
        }
    }
}
