using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terrain;
using UnityEditor;
using UnityEngine;

namespace TerrainEditors
{
    public class OpenTerrainMenuItem
    {
        private const string DefaultName = "Terrain";
        private static readonly Vector2 DefaultFirstVector = new Vector2();
        private static readonly Vector2 DefaultSecondVector = new Vector2(1, 0);

        [MenuItem("GameObject/2D Object/Terrain/Open Terrain")]
        static void Init()
        {
            var terrain = new GameObject(DefaultName);
            var view = SceneView.currentDrawingSceneView ?? SceneView.lastActiveSceneView;
            var terrainField = terrain.AddComponent<OpenTerrainField>();
            terrainField.mesh = new Mesh();
            terrain.AddComponent<MeshFilter>();
            terrain.AddComponent<MeshRenderer>();
            terrainField.AddPathNode(DefaultFirstVector);
            terrainField.AddPathNode(DefaultSecondVector);
            if (view != null)
            {
                terrain.transform.position = view.camera.transform.position;
                var position = terrain.transform.position;
                position.z = 0;
                terrain.transform.position = position;
            }
        }
    }
}
