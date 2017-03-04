using Assets.Standard_Assets.Terrain.Builder.Helper;
using Assets.Standard_Assets.Terrain.Editor.Decorator.Models;
using CommonEditors.GuiComponents.GuiComponents.CustomEditors;
using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Assets.Standard_Assets.Terrain.Builder;

namespace Assets.Standard_Assets.Terrain.Editor.Decorator
{
    public class TerrainDecoratorEditor : EditorWindow
    {
        private const string DecorationChild = "Decoration";
        private DecoratorForm _form;
        private UnityEditor.Editor _editor;

        [MenuItem("Window/2D Terrain/Decorator")]
        static void Init()
        {
            var window = EditorWindow.GetWindow<TerrainDecoratorEditor>("Terrain Decorator Editor");
        }

        private void Awake()
        {
            _form = ScriptableObject.CreateInstance<DecoratorForm>();
            _editor = UnityEditor.Editor.CreateEditor(_form, typeof(UnityEditor.Editor));
        }

        private void OnGUI()
        {
            _editor.OnInspectorGUI();

            ApplyButton();
        }

        private void ApplyButton()
        {
            GUI.enabled = _form.terrain != null;
            if (GUILayout.Button("Apply"))
            {
                ApplyDecoration();
            }
            GUI.enabled = false;
        }

        private void ApplyDecoration()
        {
            var terrain = _form.terrain;

            var decorationContainer =
                _form.decorationContainer == null
                    ? GetDefaultDecorationContainer(terrain)
                    : _form.decorationContainer;
            DestroyAllChildren(decorationContainer);
            Decorate(terrain, decorationContainer);
        }

        private void DestroyAllChildren(GameObject decorationContainer)
        {
            while(decorationContainer.transform.childCount > 0)
            {
                DestroyImmediate(decorationContainer.transform.GetChild(0).gameObject);
            }
        }

        private GameObject GetDefaultDecorationContainer(TerrainField terrain)
        {
            var decoration = terrain.transform.Find(DecorationChild);
            if(decoration == null)
            {
                decoration = new GameObject(DecorationChild).transform;
                decoration.transform.SetParent(terrain.transform, false); 
            }
            return decoration.gameObject;
        }

        private void Decorate(TerrainField terrain, GameObject decorationContainer)
        {
            var segments = terrain.BezierPathSegments.ToList();

            for (var idx = 0; idx < segments.Count; idx++)
            {
                var terrainType = 
                    TerrainBuilderAnalyzer
                        .GetTerrainTypeFromSegment(
                            segments[idx], 
                            terrain.floorTerrainMaximumSlope);

                DecorateLayer(terrain, segments[idx], decorationContainer, terrainType);
            }
        }

        private void DecorateLayer(
            TerrainField terrain,
            LineSegment2D lineSegment2D, 
            GameObject decorationContainer,
            TerrainType terrainType)
        {
            var layer = GetDecoratorLayerFor(terrainType);
            var density = UnityEngine.Random.Range(layer.minDensity, layer.maxDensity);

            if(layer.decoratorObjects.Length == 0)
            {
                return;
            }

            for(var idx = 0; idx < density; idx++)
            {
                var position = GetRandomPosition(lineSegment2D);
                var decoratorObject = GetRandomObject(layer);
                var instantiate = Instantiate(decoratorObject, position, Quaternion.identity, decorationContainer.transform);
                //instantiate.transform.SetParent(decorationContainer.transform, true);
                //instantiate.transform.position = position;
                var i = 5;
            }
        }

        private Vector2 GetRandomPosition(LineSegment2D lineSegment2D)
        {
            var min = Mathf.Min(lineSegment2D.P1.x, lineSegment2D.P2.x);
            var max = Mathf.Max(lineSegment2D.P1.x, lineSegment2D.P2.x);
            var x = UnityEngine.Random.Range(min, max);
            var y = lineSegment2D.YWhenXIs(x);
            return new Vector2(x, y);
        }

        private GameObject GetRandomObject(DecoratorLayer layer)
        {
            var idx = UnityEngine.Random.Range(0, layer.decoratorObjects.Length);
            return layer.decoratorObjects[idx].asset;
        }

        private DecoratorLayer GetDecoratorLayerFor(TerrainType terrainType)
        {
            switch (terrainType)
            {
                case TerrainType.Floor: return _form.floorLayer;
                case TerrainType.Slope: return _form.slopeLayer;
                case TerrainType.Ceiling: return _form.ceilingLayer;
            }
            return null;
        }
    }
}
