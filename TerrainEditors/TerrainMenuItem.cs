using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terrain;
using UnityEditor;
using UnityEngine;

namespace TerrainEditors
{
    public class TerrainMenuItem
    {
        private const string DefaultName = "Terrain";
        private static readonly Vector2 DefaultFirstVector = new Vector2();
        private static readonly Vector2 DefaultSecondVector = new Vector2(1, 0);

        [MenuItem("GameObject/2D Object/Terrain")]
        static void Init()
        {
            var terrain = new GameObject(DefaultName);
            var terrainField = terrain.AddComponent<TerrainField>();
            terrainField.mesh = new Mesh();
            terrain.AddComponent<MeshFilter>();
            terrain.AddComponent<MeshRenderer>();
            terrainField.AddPathNode(DefaultFirstVector);
            terrainField.AddPathNode(DefaultSecondVector);
        }
    }
}
