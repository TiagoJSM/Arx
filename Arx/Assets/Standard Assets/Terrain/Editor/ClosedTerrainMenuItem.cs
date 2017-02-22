using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Standard_Assets.Terrain.Editor
{
    public class ClosedTerrainMenuItem
    {
        private const string DefaultName = "Terrain";
        private static readonly Vector2 DefaultFirstVector = new Vector2();
        private static readonly Vector2 DefaultSecondVector = new Vector2(1, 0);
        private static readonly Vector2 DefaultThirdVector = new Vector2(0.5f, -0.5f);

        [MenuItem("GameObject/2D Object/Terrain/Closed Terrain")]
        static void Init()
        {
            var terrain = new GameObject(DefaultName);
            var view = SceneView.currentDrawingSceneView ?? SceneView.lastActiveSceneView;
            var terrainField = terrain.AddComponent<ClosedTerrainField>();
            terrainField.mesh = new Mesh();
            terrain.AddComponent<MeshFilter>();
            terrain.AddComponent<MeshRenderer>();
            terrainField.AddPathNode(DefaultFirstVector);
            terrainField.AddPathNode(DefaultSecondVector);
            terrainField.AddPathNode(DefaultThirdVector);
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
