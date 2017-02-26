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

        public Vector2 leftHandlerPosition;
        public Vector2 rightHandlerPosition;
        public Vector2 topHandlerPosition;
        public Vector2 bottomHandlerPosition;
        public Vector2 leftCapSplitHandlerPosition;
        public Vector2 rightCapSplitHandlerPosition;

        public Func<Vector2> DragHandlerGet;
        public Action<Vector2> DragHandlerSet;

        public Vector2 TopLeft
        {
            get
            {
                return new Vector2(leftHandlerPosition.x, topHandlerPosition.y);
            }
        }
        public Vector2 BottomRight
        {
            get
            {
                return new Vector2(rightHandlerPosition.x, bottomHandlerPosition.y);
            }
        }
        public float LeftCapSplit
        {
            get
            {
                return leftCapSplitHandlerPosition.x - leftHandlerPosition.x;
            }
        }
        public float RightCapSplit
        {
            get
            {
                return rightCapSplitHandlerPosition.x - leftHandlerPosition.x;
            }
        }
        
        public Rect LeftHandler
        {
            get
            {
                return CreateHandlerRect(leftHandlerPosition);
            }
        }
        public Rect RightHandler
        {
            get
            {
                return CreateHandlerRect(rightHandlerPosition);
            }
        }
        public Rect TopHandler
        {
            get
            {
                return CreateHandlerRect(topHandlerPosition);
            }
        }
        public Rect BottomHandler
        {
            get
            {
                return CreateHandlerRect(bottomHandlerPosition);
            }
        }
        public Rect LeftCapSplitHandler
        {
            get
            {
                return CreateHandlerRect(leftCapSplitHandlerPosition);
            }
        }
        public Rect RightCapSplitHandler
        {
            get
            {
                return CreateHandlerRect(rightCapSplitHandlerPosition);
            }
        }

        private Rect CreateHandlerRect(Vector2 position)
        {
            var handlerSize = new Vector2(Size, Size);
            return new Rect(position - handlerSize / 2, handlerSize);
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

            GUIUtility.GetControlID(FocusType.Passive);
            //GUIUtility.hotControl
            if (selection.TopHandler.Contains(currentEvent.mousePosition))
            {
                selection.DragHandlerGet = () => selection.topHandlerPosition;
                selection.DragHandlerSet = (position) => selection.topHandlerPosition.y = position.y;
            }
            else if (selection.BottomHandler.Contains(currentEvent.mousePosition))
            {
                selection.DragHandlerGet = () => selection.bottomHandlerPosition;
                selection.DragHandlerSet = (position) => selection.bottomHandlerPosition.y = position.y;
            }
            else if (selection.LeftHandler.Contains(currentEvent.mousePosition))
            {
                selection.DragHandlerGet = () => selection.leftHandlerPosition;
                selection.DragHandlerSet = (position) => selection.leftHandlerPosition.x = position.x;
            }
            else if (selection.LeftCapSplitHandler.Contains(currentEvent.mousePosition))
            {
                selection.DragHandlerGet = () => selection.leftCapSplitHandlerPosition;
                selection.DragHandlerSet = (position) => selection.leftCapSplitHandlerPosition.x = position.x;
            }
            else if (selection.RightCapSplitHandler.Contains(currentEvent.mousePosition))
            {
                selection.DragHandlerGet = () => selection.rightCapSplitHandlerPosition;
                selection.DragHandlerSet = (position) => selection.rightCapSplitHandlerPosition.x = position.x;
            }
            else if (selection.RightHandler.Contains(currentEvent.mousePosition))
            {
                selection.DragHandlerGet = () => selection.rightHandlerPosition;
                selection.DragHandlerSet = (position) => selection.rightHandlerPosition.x = position.x;
            }
        }

        private static bool HandleMouseDrag(TerrainTextureSelection selection)
        {
            var currentEvent = Event.current;
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
            return new TerrainTextureSelection()
            {
                leftHandlerPosition = new Vector2(0, height / 2),
                rightHandlerPosition = new Vector2(width, height / 2),
                topHandlerPosition = new Vector2(width / 2, 0),
                bottomHandlerPosition = new Vector2(width / 2, height),
                leftCapSplitHandlerPosition = new Vector2(width / 3, height / 2),
                rightCapSplitHandlerPosition = new Vector2(width * 2 / 3, height / 2)
            };
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

            selection.leftHandlerPosition = 
                LimitHorizontal(
                    selection.leftHandlerPosition, 0, selection.leftCapSplitHandlerPosition.x);
            selection.rightHandlerPosition = 
                LimitHorizontal(
                    selection.rightHandlerPosition, selection.rightCapSplitHandlerPosition.x, texture.width);
            selection.leftCapSplitHandlerPosition =
                LimitHorizontal(
                    selection.leftCapSplitHandlerPosition, selection.leftHandlerPosition.x, selection.rightCapSplitHandlerPosition.x);
            selection.rightCapSplitHandlerPosition =
                LimitHorizontal(
                    selection.rightCapSplitHandlerPosition, selection.leftCapSplitHandlerPosition.x, texture.width);
            selection.topHandlerPosition =
                LimitVertical(
                    selection.topHandlerPosition, 0, selection.bottomHandlerPosition.y);
            selection.bottomHandlerPosition =
                LimitVertical(
                    selection.bottomHandlerPosition, selection.topHandlerPosition.y, texture.height);

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
