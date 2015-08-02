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
using MathHelper.DataStructures;

namespace TerrainEditors
{
    [CustomEditor(typeof(TerrainField))]
    public class TerrainFieldEditor : Editor
    {
        private const float RectangleWidthRatio = 5;

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
            DrawPathNodesMoveHandles();
            DrawPathNodesDividerHandles();
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

        private void DrawPathNodesMoveHandles()
        {
            for (int idx = 0; idx < TerrainField.VerticeCount; idx++)
            {
                DrawPathNodeMoveHandle(idx);
            }
        }

        private void DrawPathNodesDividerHandles()
        {
            var idx = 0;
            var segmentToDivide = default(int?);
            foreach (var segment in  TerrainField.PathSegments)
            {
                var buttonPressedIndex = DrawPathNodeDividerHandle(segment, idx);
                if (segmentToDivide == null)
                {
                    segmentToDivide = buttonPressedIndex;
                }
                idx++;
            }

            if (segmentToDivide == null)
            {
                return;
            }
            TerrainField.DivideSegment(segmentToDivide.Value);
        }

        private void DrawPathNodeMoveHandle(int idx)
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

        private int? DrawPathNodeDividerHandle(LineSegment2D lineSegment, int lineIndex)
        {
            var halfLenght = (lineSegment.P2 - lineSegment.P1) / 2;
            var pressed = 
                Handles
                    .Button(
                        lineSegment.P1 + halfLenght,
                        Quaternion.identity,
                        0.2f,
                        0.2f,
                        DrawDivider);

            if (pressed)
            {
                return lineIndex;
            }
            return null;
        }

        private void AddPathNode()
        {
            var point = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin.ToVector2();
            TerrainField.AddPathNode(point);
            TerrainBuilder.BuildMeshFor(TerrainField);
        }

        private void DrawDivider(int controlID, Vector3 position, Quaternion rotation, float size)
        {
            var rectangleWidth = size / RectangleWidthRatio;

            var firstLineBottom = position + new Vector3((-size - (rectangleWidth / 2)) / 2, -size / 2);
            var secondLineBottom = position + new Vector3((size - (rectangleWidth / 2)) / 2, -size / 2);

            Handles.DrawSolidRectangleWithOutline(
                new[]
                {
                    firstLineBottom,
                    firstLineBottom + new Vector3(0, size),
                    firstLineBottom + new Vector3(rectangleWidth, size),
                    firstLineBottom + new Vector3(rectangleWidth, 0)
                },
                Color.white,
                Color.white);

            Handles.DrawSolidRectangleWithOutline(
                new[]
                {
                    secondLineBottom,
                    secondLineBottom + new Vector3(0, size),
                    secondLineBottom + new Vector3(rectangleWidth, size),
                    secondLineBottom + new Vector3(rectangleWidth, 0)
                },
                Color.white,
                Color.white);
        }
    }
}
