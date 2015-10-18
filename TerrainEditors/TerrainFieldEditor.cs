using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terrain;
using UnityEditor;
using UnityEngine;
using Extensions;
using Utils;
using CommonEditors;
using Terrain.Builder;
using Terrain.Builder.Helper;
using MathHelper.DataStructures;
using GenericComponentEditors;

namespace TerrainEditors
{
    [CustomEditor(typeof(TerrainField))]
    public class TerrainFieldEditor : NodePathEditor
    {
        private bool _requiresMeshUpdate;
        private bool _shaderChanged;
        private Shader _previousShader;

        public TerrainField TerrainField
        {
            get
            {
                return target as TerrainField;
            }
        }

        public MeshRenderer TerrainMeshRenderer
        {
            get
            {
                return TerrainField.GetComponent<MeshRenderer>();
            }
        }

        public TerrainFieldEditor()
        {
            _requiresMeshUpdate = true;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (_previousShader != TerrainField.shader)
            {
                _shaderChanged = true;
                _previousShader = TerrainField.shader;
            }
            else
            {
                _shaderChanged = false;
            }
            
            if (GUI.changed)
            {
                _requiresMeshUpdate = true;
            }
        }

        protected override void OnNodePathAdded()
        {
            TerrainBuilder.BuildMeshFor(TerrainField);
        }
        
        protected override void NodePathChanged()
        {
            _requiresMeshUpdate = true;
        }

        private void OnSceneGUI()
        {
            if (_requiresMeshUpdate)
            {
                _requiresMeshUpdate = false;
                TerrainBuilder.BuildMeshFor(TerrainField);
                TerrainColliderBuilder.BuildColliderFor(TerrainField);
            }
            if (_shaderChanged && TerrainField.shader != null)
            {
                TerrainMeshRenderer.material = new Material(TerrainField.shader);
            }
            DrawNodePathEditors();
            DrawCollider();
            HandleInput();
            
            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }

        private void DrawCollider()
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
