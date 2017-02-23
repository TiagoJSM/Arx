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
    public static class TerrainEditorUtils
    {
        private const string DefaultName = "Terrain";

        public static TTerrain InitializeTerrain<TTerrain>(bool open) where TTerrain : TerrainField
        {
            var terrain = new GameObject(DefaultName);
            var view = SceneView.currentDrawingSceneView ?? SceneView.lastActiveSceneView;
            var terrainField = terrain.AddComponent<TTerrain>();
            terrainField.mesh = new Mesh();
            terrain.AddComponent<MeshFilter>();
            terrain.AddComponent<MeshRenderer>();

            var defaultVectors = GetDefaultVectors(open, view);

            var position = defaultVectors.First().ToVector3();
            position.z = 0;
            terrain.transform.position = position;

            foreach (var defaultVector in defaultVectors)
            {
                terrainField.AddPathNode(defaultVector);
            }

            Selection.activeObject = terrainField.gameObject;

            return terrainField;
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
