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

namespace GenericComponentEditors
{
    public abstract class NodePathEditor : Editor
    {
        private const float RectangleWidthRatio = 5;
        private readonly Color EndColor = new Color(1, 1 / 3, 0);
        private readonly Color StartColor = Color.green;

        private EditorInputHandler _inputHandler;

        public NodePath NodePath
        {
            get
            {
                return target as NodePath;
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
                    new InputCombination(AddPathNode, MouseButton.Right, EventModifiers.Control));
        }

        protected void DrawNodePathEditors()
        {
            DrawPathNodesMoveHandles();
            DrawPathNodesDividerHandles();
            DrawLinesBetweenPathNodes();
            if (NodePath.useBezier)
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

        private void DrawPathNodesMoveHandles()
        {
            for (int idx = 0; idx < NodePath.VerticeCount; idx++)
            {
                var point = NodePath[idx];
                var translated = DrawPathNodeMoveHandle(point, Color.white);
                if (point != translated)
                {
                    NodePath[idx] = translated;
                    NodePathChanged();
                }
            }
        }

        private void DrawPathNodesDividerHandles()
        {
            var idx = 0;
            var segmentToDivide = default(int?);
            foreach (var segment in NodePath.ControlPathSegments)
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
        }

        private Vector2 DrawPathNodeMoveHandle(Vector2 point, Color color)
        {
            Handles.color = color;
            Handles.DrawSolidArc(point.ToVector3(), new Vector3(0, 0, -1), Vector3.right, 360, 0.2f);
            var translated =
                Handles
                    .FreeMoveHandle(
                        point,
                        Quaternion.identity,
                        0.2f,
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
            foreach (var segment in NodePath.ControlPathSegments)
            {
                Handles.DrawLine(segment.P1.ToVector3(), segment.P2.ToVector3());
            }
        }

        private void AddPathNode()
        {
            var point = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin.ToVector2();
            NodePath.AddPathNode(point);
            OnNodePathAdded();
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
                var point = NodePath.GetBezierControlPointAt(idx);
                var translated = DrawPathNodeMoveHandle(point, color);
                if (point != translated)
                {
                    NodePath.SetBezierControlPointAt(idx, translated);
                    NodePathChanged();
                }
            }
        }

        private void DrawBezierConnectionLines()
        {
            foreach (var bezierLineSegment in NodePath.BezierPathSegments)
            {
                Handles.color = StartColor;
                Handles.DrawLine(bezierLineSegment.LineSegment.P1.ToVector3(), bezierLineSegment.P1ControlPoint.ToVector3());
                Handles.color = EndColor;
                Handles.DrawLine(bezierLineSegment.LineSegment.P2.ToVector3(), bezierLineSegment.P2ControlPoint.ToVector3());
            }
        }
    }
}
