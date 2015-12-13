using Extensions;
using GenericComponentEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terrain;
using UnityEditor;
using UnityEngine;

namespace TerrainEditors
{
    public abstract class TerrainFieldEditor<TTerrain> : NodePathEditor where TTerrain : TerrainField
    {
        private Shader _previousShader;

        public TTerrain TerrainField
        {
            get
            {
                return target as TTerrain;
            }
        }

        public MeshRenderer TerrainMeshRenderer
        {
            get
            {
                return TerrainField.GetComponent<MeshRenderer>();
            }
        }

        protected bool RequiresMeshUpdate { get; set; }

        public TerrainFieldEditor()
        {
            RequiresMeshUpdate = true;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (_previousShader != TerrainField.shader && TerrainField.shader != null)
            {
                TerrainMeshRenderer.material = new Material(TerrainField.shader);
            }
        }

        protected void DrawCollider()
        {
            var collider = TerrainField.GetComponent<EdgeCollider2D>();
            if (collider == null)
            {
                return;
            }
            var terrainPosition = TerrainField.transform.position;

            var points =
                collider
                    .points
                    .Select(p => p.ToVector3() + terrainPosition)
                    .ToArray();

            Handles.color = Color.green;
            Handles.DrawAAPolyLine(3f, points);
        }
    }
}
