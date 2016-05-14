using GenericComponents.Waypoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace GenericComponentEditors.Waypoint
{
    [CustomEditor(typeof(WaypointPath))]
    public class WaypointPathEditor : NodePathEditor
    {
        private const string DefaultName = "Waypoint Path";
        private static readonly Vector2 DefaultFirstVector = new Vector2();
        private static readonly Vector2 DefaultSecondVector = new Vector2(1, 0);

        [MenuItem("GameObject/2D Object/Waypoint path")]
        static void Init()
        {
            var waypointPathGO = new GameObject(DefaultName);
            var waypointPath = waypointPathGO.AddComponent<WaypointPath>();
            waypointPath.AddPathNode(DefaultFirstVector);
            waypointPath.AddPathNode(DefaultSecondVector);
        }

        protected override void NodePathChanged()
        {
            //throw new NotImplementedException();
        }

        protected override void OnNodePathAdded()
        {
            //throw new NotImplementedException();
        }

        protected override void OnNodePathRemoved()
        {
            //throw new NotImplementedException();
        }

        private void OnSceneGUI()
        {
            DrawNodePathEditors();
            HandleInput();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
