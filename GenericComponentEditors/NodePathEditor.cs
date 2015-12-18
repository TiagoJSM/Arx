using GenericComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Extensions;
using MathHelper.DataStructures;
using Utils;
using CommonEditors;
using GenericComponents.Behaviours;

namespace GenericComponentEditors
{
    public abstract class NodePathEditor : Editor
    {
        private const float RectangleWidthRatio = 5;
        private const float MoveHandleSize = 0.2f;
        private readonly Color EndColor = new Color(1, 1 / 3, 0);
        private readonly Color StartColor = Color.green;

        private EditorInputHandler _inputHandler;

        public NodePathBehaviour NodePathBehaviour
        {
            get
            {
                return target as NodePathBehaviour;
            }
        }

        public NodePath NodePath
        {
            get
            {
                return NodePathBehaviour.NodePath;
            }
        }

        protected EditorInputHandler InputHandler
        {
            get
            {
                return _inputHandler;
            }
        }

        public NodePathEditor()
        {
            _inputHandler =
                new EditorInputHandler(
                    new InputCombination(AddPathNode, MouseButton.Right, EventModifiers.Control),
                    new InputCombination(RemovePathNode, MouseButton.Right, EventModifiers.Shift));
        }

        public override void OnInspectorGUI()
        {
            NodePath.UseBezier = EditorGUILayout.Toggle("Use Bezier", NodePath.UseBezier);
            NodePath.BezierDivisions = EditorGUILayout.IntField("Bezier Divisions", NodePath.BezierDivisions);
            base.OnInspectorGUI();
        }

        protected void DrawNodePathEditors()
        {
            DrawPathNodesMoveHandles();
            DrawPathNodesDividerHandles();
            DrawLinesBetweenPathNodes();
            if (NodePath.UseBezier)
            {
                DrawBezierMoveHandles();
                DrawBezierConnectionLines();
            }
        }

        protected void HandleInput()
        {
            _inputHandler.HandleInput();
        }

        protected abstract void NodePathChanged();
        protected abstract void OnNodePathAdded();
        protected abstract void OnNodePathRemoved();

        private void DrawPathNodesMoveHandles()
        {
            for (int idx = 0; idx < NodePath.VerticeCount; idx++)
            {
                var point = NodePathBehaviour[idx];
                var translated = DrawPathNodeMoveHandle(point, Color.white);
                if (point != translated)
                {
                    NodePathBehaviour[idx] = translated;
                    NodePathChanged();
                }
            }
        }

        private void DrawPathNodesDividerHandles()
        {
            var idx = 0;
            var segmentToDivide = default(int?);
            foreach (var segment in NodePathBehaviour.InScenePathSegments)
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
            NodePath.DivideSegment(segmentToDivide.Value);
            /*if (IsCircularPath)
            {
                var firstNode = NodePathBehaviour.InScenePathNodes.First();
                var lastNode = NodePathBehaviour.InScenePathNodes.Last();
                var segment = new LineSegment2D(lastNode, firstNode);
                var buttonPressedIndex = DrawPathNodeDividerHandle(segment, idx);
                if (buttonPressedIndex != null)
                {
                    NodePathBehaviour.AddPathNode(segment.HalfPoint);
                }
            }*/
        }

        private Vector2 DrawPathNodeMoveHandle(Vector2 point, Color color)
        {
            Handles.color = color;
            Handles.DrawSolidArc(point.ToVector3(), new Vector3(0, 0, -1), Vector3.right, 360, MoveHandleSize);
            var translated =
                Handles
                    .FreeMoveHandle(
                        point,
                        Quaternion.identity,
                        MoveHandleSize,
                        Vector3.zero,
                        Handles.RectangleCap)
                        .ToVector2();
            
            return translated;
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

        private void DrawLinesBetweenPathNodes()
        {
            Handles.color = Color.blue;
            foreach (var segment in NodePathBehaviour.InScenePathSegments)
            {
                Handles.DrawLine(segment.P1.ToVector3(), segment.P2.ToVector3());
            }
            /*if (IsCircularPath)
            {
                var firstNode = NodePathBehaviour.InScenePathNodes.First();
                var lastNode = NodePathBehaviour.InScenePathNodes.Last();
                Handles.DrawLine(lastNode.ToVector3(), firstNode.ToVector3());
            }*/
        }

        private void AddPathNode()
        {
            var point = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin.ToVector2();
            AddPathNode(point);
        }

        private void AddPathNode(Vector2 point)
        {
            NodePathBehaviour.AddPathNode(point);
            OnNodePathAdded();
        }

        private void RemovePathNode()
        {
            if(NodePath.VerticeCount <= 2)
            {
                return;
            }
            var point = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin.ToVector2();
            var pointInRadius = NodePathBehaviour.Any(v => v.IsInRadius(point, MoveHandleSize));
            if (!pointInRadius)
            {
                return;
            }
            var index = NodePathBehaviour.IndexOfMin(v => Vector2.Distance(v, point));
            NodePath.RemovePathNodeAt(index.Value);
            OnNodePathRemoved();
        }

        private void DrawBezierMoveHandles()
        {
            for (int idx = 0; idx < NodePath.BezierControlPointsCount; idx++)
            {
                var color = new Color(1, 1 / 3, 0);
                if (idx % 2 == 0)
                {
                    color = Color.green;
                }
                var point = NodePathBehaviour.GetBezierControlPointAt(idx);
                var translated = DrawPathNodeMoveHandle(point, color);
                if (point != translated)
                {
                    NodePathBehaviour.SetBezierControlPointAt(idx, translated);
                    NodePathChanged();
                }
            }
        }

        private void DrawBezierConnectionLines()
        {
            foreach (var bezierLineSegment in NodePathBehaviour.BezierPathSegments)
            {
                Handles.color = StartColor;
                Handles.DrawLine(bezierLineSegment.LineSegment.P1.ToVector3(), bezierLineSegment.P1ControlPoint.ToVector3());
                Handles.color = EndColor;
                Handles.DrawLine(bezierLineSegment.LineSegment.P2.ToVector3(), bezierLineSegment.P2ControlPoint.ToVector3());
            }
        }
    }
}
