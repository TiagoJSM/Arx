using Assets.Standard_Assets.Extensions;
using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Standard_Assets.Terrain.Editor.Utils
{
    public class TerrainTextureSelection
    {
        public static readonly float Size = 20;

        private Vector2 _topLeft;
        private Vector2 _bottomRight;
        private float _leftCapSplit;
        private float _rightCapSplit;

        public Vector2 LeftHandlerPosition
        {
            get
            {
                return new Vector2(_topLeft.x, ((_bottomRight.y - _topLeft.y) / 2) + _topLeft.y);
            }
            set
            {
                var difference = value.x - _topLeft.x;
                _leftCapSplit -= difference;
                _rightCapSplit -= difference;
                if(_leftCapSplit < 0)
                {
                    _leftCapSplit = 0;
                }
                if(_rightCapSplit < 0)
                {
                    _rightCapSplit = 0;
                }
                _topLeft.x = value.x;
                if(_topLeft.x > _bottomRight.x)
                {
                    _topLeft.x = _bottomRight.x;
                }
            }
        }
        public Vector2 RightHandlerPosition
        {
            get
            {
                return new Vector2(_bottomRight.x, ((_bottomRight.y - _topLeft.y) / 2) + _topLeft.y);
            }
            set
            {
                _bottomRight.x = value.x;
            }
        }
        public Vector2 TopHandlerPosition
        {
            get
            {
                return new Vector2(((_bottomRight.x - _topLeft.x) / 2) + _topLeft.x, _topLeft.y);
            }
            set
            {
                _topLeft.y = value.y;
            }
        }
        public Vector2 BottomHandlerPosition
        {
            get
            {
                return new Vector2(((_bottomRight.x - _topLeft.x) / 2) + _topLeft.x, _bottomRight.y);
            }
            set
            {
                _bottomRight.y = value.y;
            }
        }
        public Vector2 LeftCapSplitHandlerPosition
        {
            get
            {
                var leftCapSplitHandler = LeftHandlerPosition;
                leftCapSplitHandler.x += _leftCapSplit;
                return leftCapSplitHandler;
            }
            set
            {
                _leftCapSplit = value.x - LeftHandlerPosition.x;
            }
        }
        public Vector2 RightCapSplitHandlerPosition
        {
            get
            {
                var rightCapSplitHandler = LeftHandlerPosition;
                rightCapSplitHandler.x += _rightCapSplit;
                return rightCapSplitHandler;
            }
            set
            {
                _rightCapSplit = value.x - LeftHandlerPosition.x;
            }
        }

        public Func<Vector2> DragHandlerGet;
        public Action<Vector2> DragHandlerSet;

        public Vector2 TopLeft
        {
            get
            {
                return _topLeft;
            }
        }
        public Vector2 BottomRight
        {
            get
            {
                return _bottomRight;
            }
        }
        public float LeftCapSplit
        {
            get
            {
                return _leftCapSplit;
            }
        }
        public float RightCapSplit
        {
            get
            {
                return _rightCapSplit;
            }
        }

        public Rect LeftHandler
        {
            get
            {
                return CreateHandlerRect(LeftHandlerPosition);
            }
        }
        public Rect RightHandler
        {
            get
            {
                return CreateHandlerRect(RightHandlerPosition);
            }
        }
        public Rect TopHandler
        {
            get
            {
                return CreateHandlerRect(TopHandlerPosition);
            }
        }
        public Rect BottomHandler
        {
            get
            {
                return CreateHandlerRect(BottomHandlerPosition);
            }
        }
        public Rect LeftCapSplitHandler
        {
            get
            {
                return CreateHandlerRect(LeftCapSplitHandlerPosition);
            }
        }
        public Rect RightCapSplitHandler
        {
            get
            {
                return CreateHandlerRect(RightCapSplitHandlerPosition);
            }
        }

        private Rect CreateHandlerRect(Vector2 position)
        {
            var handlerSize = new Vector2(Size, Size);
            return new Rect(position - handlerSize / 2, handlerSize);
        }

        public TerrainTextureSelection(
            Vector2 topLeft, Vector2 bottomRight, float leftCapSplit, float rightCapSplit)
        {
            _topLeft = topLeft;
            _bottomRight = bottomRight;
            _leftCapSplit = leftCapSplit;
            _rightCapSplit = rightCapSplit;
        }
    }

    public static class TerrainEditorUtils
    {
        private const string DefaultName = "Terrain";
        private const string DefaultMaterialPath = "Assets/Standard Assets/Terrain/Materials/Default.mat";

        private static GUIStyle _whiteBackground;

        private static GUIStyle WhiteBackground
        {
            get
            {
                if(_whiteBackground == null)
                {
                    _whiteBackground = new GUIStyle(GUI.skin.box);
                    _whiteBackground.normal.background = EditorGUIUtility.whiteTexture;
                }
                return _whiteBackground;
            }
        }

        public static TTerrain InitializeTerrain<TTerrain>(bool open) where TTerrain : TerrainField
        {
            var defaultMaterial = AssetDatabase.LoadAssetAtPath<Material>(DefaultMaterialPath);
            var terrain = new GameObject(DefaultName);
            var view = SceneView.currentDrawingSceneView ?? SceneView.lastActiveSceneView;
            var terrainField = terrain.AddComponent<TTerrain>();
            terrainField.mesh = new Mesh();
            terrain.AddComponent<MeshFilter>();
            var renderer = terrain.AddComponent<MeshRenderer>();

            var defaultVectors = GetDefaultVectors(open, view);

            var position = defaultVectors.First().ToVector3();
            position.z = 0;
            terrain.transform.position = position;

            foreach (var defaultVector in defaultVectors)
            {
                terrainField.AddPathNode(defaultVector);
            }

            renderer.material = defaultMaterial;
            Selection.activeObject = terrainField.gameObject;

            return terrainField;
        }

        public static bool TextureSelection(TerrainTextureSelection selection, Texture texture)
        {
            Handles.BeginGUI();
            HandleMouseButtonEvents(selection);
            DrawSelectionLines(selection);
            var dragged = DrawDragRectangles(selection, texture);
            Handles.EndGUI();
            return dragged;
        }

        private static void HandleMouseButtonEvents(TerrainTextureSelection selection)
        {
            var currentEvent = Event.current;
            if(currentEvent == null)
            {
                return;
            }

            if (currentEvent.type == EventType.MouseUp)
            {
                selection.DragHandlerGet = null;
                selection.DragHandlerSet = null;
            }

            if (currentEvent.type != EventType.mouseDown)
            {
                return;
            }

            if (selection.TopHandler.Contains(currentEvent.mousePosition))
            {
                selection.DragHandlerGet = () => selection.TopHandlerPosition;
                selection.DragHandlerSet = (position) => selection.TopHandlerPosition = position;
            }
            else if (selection.BottomHandler.Contains(currentEvent.mousePosition))
            {
                selection.DragHandlerGet = () => selection.BottomHandlerPosition;
                selection.DragHandlerSet = (position) => selection.BottomHandlerPosition = position;
            }
            else if (selection.LeftHandler.Contains(currentEvent.mousePosition))
            {
                selection.DragHandlerGet = () => selection.LeftHandlerPosition;
                selection.DragHandlerSet = (position) => selection.LeftHandlerPosition = position;
            }
            else if (selection.LeftCapSplitHandler.Contains(currentEvent.mousePosition))
            {
                selection.DragHandlerGet = () => selection.LeftCapSplitHandlerPosition;
                selection.DragHandlerSet = (position) => selection.LeftCapSplitHandlerPosition = position;
            }
            else if (selection.RightCapSplitHandler.Contains(currentEvent.mousePosition))
            {
                selection.DragHandlerGet = () => selection.RightCapSplitHandlerPosition;
                selection.DragHandlerSet = (position) => selection.RightCapSplitHandlerPosition = position;
            }
            else if (selection.RightHandler.Contains(currentEvent.mousePosition))
            {
                selection.DragHandlerGet = () => selection.RightHandlerPosition;
                selection.DragHandlerSet = (position) => selection.RightHandlerPosition = position;
            }
        }

        private static bool HandleMouseDrag(TerrainTextureSelection selection)
        {
            var currentEvent = Event.current;
            if (selection.DragHandlerGet == null)
            {
                return false;
            }
            if (currentEvent == null || selection.DragHandlerGet == null)
            {
                return false;
            }

            if (currentEvent.type == EventType.MouseDrag)
            {
                selection.DragHandlerSet(selection.DragHandlerGet() + currentEvent.delta);
                return true;
            }
            return false;
        }

        public static TerrainTextureSelection GenerateTerrainTextureSelection(float width, float height)
        {
            return new TerrainTextureSelection(Vector2.zero, new Vector2(width, height), width / 3, width * 2 / 3);
        }

        private static void DrawSelectionLines(TerrainTextureSelection selection)
        {
            var topLeft = selection.TopLeft;
            var topRight = new Vector2(selection.BottomRight.x, selection.TopLeft.y);
            var bottomLeft = new Vector2(selection.TopLeft.x, selection.BottomRight.y);
            var bottomRight = selection.BottomRight;

            //bounds
            DrawSelectionLine(topLeft, topRight);
            DrawSelectionLine(topRight, bottomRight);
            DrawSelectionLine(bottomRight, bottomLeft);
            DrawSelectionLine(bottomLeft, topLeft);

            //caps
            var leftCapX = new Vector2(selection.LeftCapSplit, 0);
            var rightCapX = new Vector2(selection.RightCapSplit, 0);
            DrawSelectionLine(topLeft + leftCapX, bottomLeft + leftCapX);
            DrawSelectionLine(topLeft + rightCapX, bottomLeft + rightCapX);
        }

        private static void DrawSelectionLine(Vector2 p1, Vector2 p2)
        {
            Handles.DrawLine(p1, p2);
        }

        private static bool DrawDragRectangles(TerrainTextureSelection selection, Texture texture)
        {
            var dragged = HandleMouseDrag(selection);

            if (dragged)
            {
                selection.LeftHandlerPosition =
                    LimitHorizontal(
                        selection.LeftHandlerPosition, 0, selection.LeftCapSplitHandlerPosition.x);
                selection.RightHandlerPosition =
                    LimitHorizontal(
                        selection.RightHandlerPosition, selection.RightCapSplitHandlerPosition.x, texture.width);
                selection.LeftCapSplitHandlerPosition =
                    LimitHorizontal(
                        selection.LeftCapSplitHandlerPosition, selection.LeftHandlerPosition.x, selection.RightCapSplitHandlerPosition.x);
                selection.RightCapSplitHandlerPosition =
                    LimitHorizontal(
                        selection.RightCapSplitHandlerPosition, selection.LeftCapSplitHandlerPosition.x, texture.width);
                selection.TopHandlerPosition =
                    LimitVertical(
                        selection.TopHandlerPosition, 0, selection.BottomHandlerPosition.y);
                selection.BottomHandlerPosition =
                    LimitVertical(
                        selection.BottomHandlerPosition, selection.TopHandlerPosition.y, texture.height);
            }

            DrawDragRectangle(selection.LeftHandler);
            DrawDragRectangle(selection.RightHandler);
            DrawDragRectangle(selection.LeftCapSplitHandler);
            DrawDragRectangle(selection.RightCapSplitHandler);
            DrawDragRectangle(selection.TopHandler);
            DrawDragRectangle(selection.BottomHandler);

            return dragged;
        }

        private static Vector2 LimitHorizontal(Vector2 position, float min, float max)
        {
            return new Vector2(Mathf.Clamp(position.x, min, max), position.y);
        }

        private static Vector2 LimitVertical(Vector2 position, float min, float max)
        {
            return new Vector2(position.x, Mathf.Clamp(position.y, min, max));
        }

        private static void DrawDragRectangle(Rect handler)
        {
            GUILayout.BeginArea(handler, WhiteBackground);
            GUILayout.EndArea();
            EditorGUIUtility.AddCursorRect(handler, MouseCursor.Pan);
        }

        private static Vector2[] GetDefaultVectors(bool open, SceneView view)
        {
            if(view == null)
            {
                return open
                    ? new[] { new Vector2(), new Vector2(1, 0) }
                    : new[] { new Vector2(), new Vector2(1, 0), new Vector2(0.5f, -0.5f) };
            }

            var ratio = 0.1f;
            var bounds = view.camera.OrthographicBounds();
            var width = bounds.max.x - bounds.min.x;
            var height = bounds.max.y - bounds.min.y;

            var left = new Vector2(bounds.min.x + width * ratio, bounds.min.y + height * ratio);
            var right = new Vector2(bounds.max.x - width * ratio, bounds.min.y + height * ratio);

            return open
                ? new[] { left, right }
                : new[] { left, bounds.center.ToVector2(), right };
        }
    }
}
