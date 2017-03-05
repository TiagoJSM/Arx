using Assets.Standard_Assets.GenericComponents.Behaviours;
using Assets.Standard_Assets.Utility.Editor;
using CommonEditors;
using Extensions;
using MathHelper;
using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Standard_Assets.GenericComponents.Editor
{
    public abstract class NodePathEditor : UnityEditor.Editor
    {
        private const float RectangleWidthRatio = 5;
        private const float handlesSizeRatio = 10;
        private readonly Color EndColor = new Color(1, 1 / 3, 0);
        private readonly Color StartColor = Color.green;

        private bool _showBezier;
        private EditorInputHandler _inputHandler;

        private Vector2? _markeeStart;
        private Vector2 _markeeEnd;
        private int[] _selectedNodeIndices;

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
            base.OnInspectorGUI();
        }

        protected void DrawNodePathEditors()
        {
            DrawPathNodesMoveHandles();
            DrawPathNodesDividerHandles();
            DrawLinesBetweenPathNodes();
            if (NodePath.UseBezier && _showBezier)
            {
                DrawBezierMoveHandles();
                DrawBezierConnectionLines();
            }
        }

        protected void HandleInput()
        {
            var e = Event.current;

            _inputHandler.HandleInput();
            HandleBezierToggle();
            HandleMarkeeSelection();
            HandleDeselection();
        }

        protected abstract void NodePathChanged();
        protected abstract void OnNodePathAdded();
        protected abstract void OnNodePathRemoved();

        private void DrawPathNodesMoveHandles()
        {
            if(_selectedNodeIndices != null)
            {
                DrawMultiSelectionPathNodesMoveHandles();
            }
            else
            {
                DrawSinglePathNodeMoveHandles();
            }
        }

        private void DrawSinglePathNodeMoveHandles()
        {
            for (int idx = 0; idx < NodePath.VerticeCount; idx++)
            {
                var point = NodePathBehaviour[idx];
                var translated = DrawPathNodeMoveHandle(point, Color.white, Color.white);
                if (point != translated)
                {
                    Undo.RecordObject(target, "Move path node");
                    NodePathBehaviour[idx] = translated;
                    NodePathChanged();
                }
            }
        }

        private void DrawMultiSelectionPathNodesMoveHandles()
        {
            Vector2? translation = null;
            foreach (var index in _selectedNodeIndices)
            {
                var point = NodePathBehaviour[index];
                var translated = DrawPathNodeMoveHandle(point, Color.white, Color.green);
                if (point != translated && translation == null)
                {
                    translation = translated - point;
                }
            }

            if(translation == null)
            {
                return;
            }

            Undo.RecordObject(target, "Move multiple path nodes");
            foreach (var index in _selectedNodeIndices)
            {
                NodePathBehaviour[index] += translation.Value;
            }
            NodePathChanged();
        }

        private void DrawPathNodesDividerHandles()
        {
            var idx = 0;
            var segmentToDivide = default(int?);
            foreach (var segment in NodePathBehaviour.BezierLinePathSegments)
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
            OnNodePathAdded();
        }

        private Vector2 DrawPathNodeMoveHandle(Vector2 point, Color arcColor, Color moveHandleColor)
        {
            Handles.color = arcColor;
            var size = HandleUtility.GetHandleSize(point) / handlesSizeRatio;
            Handles.DrawSolidArc(point.ToVector3(), new Vector3(0, 0, -1), Vector3.right, 360, size);
            Handles.color = moveHandleColor;
            var translated =
                Handles
                    .FreeMoveHandle(
                        point,
                        Quaternion.identity,
                        size,
                        Vector3.zero,
                        Handles.RectangleCap)
                        .ToVector2();

            return translated;
        }

        private int? DrawPathNodeDividerHandle(BezierLineSegment2D bezierLineSegment, int lineIndex)
        {
            var lineSegment = bezierLineSegment.LineSegment;
            var middlePoint =
                Bezier
                    .CalculateBezierPoint(
                        0.5f,
                        lineSegment.P1,
                        bezierLineSegment.P1ControlPoint,
                        bezierLineSegment.P2ControlPoint,
                        lineSegment.P2);
            var halfLenght = (lineSegment.P2 - lineSegment.P1) / 2;
            var size = HandleUtility.GetHandleSize(middlePoint) / handlesSizeRatio;
            var pressed =
                Handles
                    .Button(
                        middlePoint,
                        Quaternion.identity,
                        size,
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
            foreach (var segment in NodePathBehaviour.BezierPathSegments)
            {
                Handles.DrawLine(segment.P1, segment.P2);
            }
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
            if (NodePath.VerticeCount <= 2)
            {
                return;
            }
            var point = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin.ToVector2();
            var size = HandleUtility.GetHandleSize(point) / handlesSizeRatio;
            var pointInRadius = NodePathBehaviour.Any(v => v.IsInRadius(point, size));
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
                var translated = DrawPathNodeMoveHandle(point, color, color);
                if (point != translated)
                {
                    Undo.RecordObject(target, "Move bezier point");
                    NodePathBehaviour.SetBezierControlPointAt(idx, translated);
                    NodePathChanged();
                }
            }
        }

        private void DrawBezierConnectionLines()
        {
            foreach (var bezierLineSegment in NodePathBehaviour.BezierLinePathSegments)
            {
                Handles.color = StartColor;
                Handles.DrawLine(bezierLineSegment.LineSegment.P1.ToVector3(), bezierLineSegment.P1ControlPoint.ToVector3());
                Handles.color = EndColor;
                Handles.DrawLine(bezierLineSegment.LineSegment.P2.ToVector3(), bezierLineSegment.P2ControlPoint.ToVector3());
            }
        }

        private void HandleMarkeeSelection()
        {
            var e = Event.current;

            if (!e.control && e.type == EventType.MouseDown && e.button == (int)MouseButton.Left)
            {
                _selectedNodeIndices = null;
            }

            if (e.control && e.type == EventType.MouseDown && e.button == (int)MouseButton.Left)
            {
                _markeeStart = GetMousePositionInWorld();
            }
            if (e.control && _markeeStart != null)
            {
                _markeeEnd = GetMousePositionInWorld();
                _selectedNodeIndices = GetSelectedMarkeeSelectedIndices();
            }
            if (_markeeStart != null && e.type == EventType.keyUp && e.keyCode == KeyCode.LeftControl)
            {
                _markeeStart = null;
            }
        }

        private void HandleBezierToggle()
        {
            var e = Event.current;
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Space)
            {
                _showBezier = !_showBezier;
            }
        }

        private void HandleDeselection()
        {
            var e = Event.current;
            if (e.keyCode == KeyCode.Escape)
            {
                Selection.activeGameObject = null;
            }
            else
            {
                Selection.activeGameObject = NodePathBehaviour.transform.gameObject;
            }
        }

        private Vector2 GetMousePositionInWorld()
        {
            var e = Event.current;
            return HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;
        }

        private int[] GetSelectedMarkeeSelectedIndices()
        {
            if (_markeeStart == null)
            {
                return null;
            }

            var selection = new MarkeeSelectionRect()
            {
                p1 = _markeeStart.Value,
                p2 = _markeeEnd
            };
            return MarkeeSelection.GetMarkeeSelectionIndices(selection, NodePathBehaviour.PathNodes);
        }
    }
}
