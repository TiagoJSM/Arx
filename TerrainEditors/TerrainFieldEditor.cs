using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terrain;
using UnityEditor;
using UnityEngine;
using Extensions;
using Utils;
using CommonEditors;
using Terrain.Builder;
using Terrain.Builder.Helper;

namespace TerrainEditors
{
    [CustomEditor(typeof(TerrainField))]
    public class TerrainFieldEditor : Editor
    {
        private EditorInputHandler _inputHandler;
        private bool _requiresMeshUpdate;

        public TerrainField TerrainField
        {
            get
            {
                return target as TerrainField;
            }
        }

        public TerrainFieldEditor()
        {
            _requiresMeshUpdate = true;
            _inputHandler = 
                new EditorInputHandler(
                    new InputCombination(AddPathNode, MouseButton.Right, EventModifiers.Control));
        }

        private void OnSceneGUI()
        {
            //GUILayout.Button("something");
            if (_requiresMeshUpdate)
            {
                _requiresMeshUpdate = false;
                TerrainBuilder.BuildMeshFor(TerrainField);
                TerrainColliderBuilder.BuildColliderFor(TerrainField);
            }
            DrawLinesBetweenPathNodes();
            DrawPathNodesHandles();
            HandleInput();
            
            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }

        private void DrawLinesBetweenPathNodes()
        {
            foreach (var segment in TerrainField.PathSegments)
            {
                Handles.DrawLine(segment.P1.ToVector3(), segment.P2.ToVector3());
            }
        }

        private void HandleInput()
        {
            _inputHandler.HandleInput();
        }

        private void DrawPathNodesHandles()
        {
            for (int idx = 0; idx < TerrainField.VerticeCount; idx++)
            {
                DrawPathNodeHandle(idx);
            }
        }

        private void DrawPathNodeHandle(int idx)
        {
            Handles.DrawSolidArc(TerrainField[idx].ToVector3(), new Vector3(0, 0, -1), Vector3.right, 360, 0.2f);
            var translated =
                Handles
                    .FreeMoveHandle(
                        TerrainField[idx],
                        Quaternion.identity,
                        0.2f,
                        Vector3.zero,
                        Handles.RectangleCap)
                        .ToVector2();
            if (TerrainField[idx] != translated)
            {
                TerrainField[idx] = translated;
                _requiresMeshUpdate = true;
            }
        }

        private void AddPathNode()
        {
            var point = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin.ToVector2();
            TerrainField.AddPathNode(point);
            TerrainBuilder.BuildMeshFor(TerrainField);
        }
    }
}
