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
            Handles.color = Color.yellow;
            var point = NewTargetZone.Position;
            var size = HandleUtility.GetHandleSize(point) / 5f;
            Handles.DrawSolidArc(point, new Vector3(0, 0, -1), Vector3.right, 360, size);
            var translated =
                Handles
                    .FreeMoveHandle(
                        point,
                        Quaternion.identity,
                        size,
                        Vector3.zero,
                        Handles.RectangleCap);

            if(point != translated)
            {
                Undo.RecordObject(target, "Move camera target");
                NewTargetZone.Position = translated;
            }
        }
    }
}
