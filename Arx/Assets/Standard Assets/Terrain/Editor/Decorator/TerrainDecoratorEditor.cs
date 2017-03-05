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
        private Vector2 _scroll;

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
            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            _editor.OnInspectorGUI();
            EditorGUILayout.EndScrollView();

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
                var rotation = GetRandomRotation(lineSegment2D, decoratorObject);
                var scale = GetRandomScale(decoratorObject);
                var instantiate = 
                    Instantiate(
                        decoratorObject.asset, position,
                        rotation, 
                        decorationContainer.transform);
                instantiate.transform.localScale = scale;
            }
        }

        private Vector3 GetRandomScale(DecoratorObject decoratorObject)
        {
            var minScaleVariation = ValidateScale(decoratorObject.minScaleVariation);
            var maxScaleVariation = ValidateScale(decoratorObject.maxScaleVariation);

            var x = UnityEngine.Random.Range(minScaleVariation.x, maxScaleVariation.x);
            var y = UnityEngine.Random.Range(minScaleVariation.y, maxScaleVariation.y);

            return new Vector3(
                x == 0 ? 1 : x,
                y == 0 ? 1 : y,
                1);
        }

        private Vector2 ValidateScale(Vector2 scale)
        {
            if (scale.x == 0)
            {
                scale.x = 1;
                Debug.Log("Scale can't be 0, defaulted to 1");
            }
            if (scale.y == 0)
            {
                scale.y = 1;
                Debug.Log("Scale can't be 0, defaulted to 1");
            }
            return scale;
        }

        private Quaternion GetRandomRotation(LineSegment2D lineSegment2D, DecoratorObject decoratorObject)
        {
            var rotation = Vector2.Angle(Vector2.up, lineSegment2D.NormalVector);
            if(lineSegment2D.NormalVector.x > 0)
            {
                rotation += 180;
            }
            var randomRotation = UnityEngine.Random.Range(-decoratorObject.rotationVariation, decoratorObject.rotationVariation);
            return Quaternion.Euler(0, 0, randomRotation + rotation);
        }

        private Vector2 GetRandomPosition(LineSegment2D lineSegment2D)
        {
            var min = Mathf.Min(lineSegment2D.P1.x, lineSegment2D.P2.x);
            var max = Mathf.Max(lineSegment2D.P1.x, lineSegment2D.P2.x);
            var x = UnityEngine.Random.Range(min, max);
            var y = lineSegment2D.YWhenXIs(x);
            return new Vector2(x, y);
        }

        private DecoratorObject GetRandomObject(DecoratorLayer layer)
        {
            var idx = UnityEngine.Random.Range(0, layer.decoratorObjects.Length);
            return layer.decoratorObjects[idx];
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
