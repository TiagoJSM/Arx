using _2DDynamicCamera.Zones;
using _2DDynamicCamera.Zones.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace _2DDynamicCameraEditors.Editors
{
    [CustomEditor(typeof(NewTargetZone), true)]
    public class NewTargetZoneEditor : Editor
    {
        public NewTargetZone NewTargetZone
        {
            get
            {
                return target as NewTargetZone;
            }
        }

        void OnSceneGUI()
        {
            NewTargetZone.Position =
                Handles
                    .PositionHandle(
                        NewTargetZone.Position,
                        Quaternion.identity);
        }
    }
}
