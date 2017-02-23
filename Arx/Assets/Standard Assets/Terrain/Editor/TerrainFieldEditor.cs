using Assets.Standard_Assets.Terrain.Builder;
using CommonEditors;
using Extensions;
using GenericComponentEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Standard_Assets.Terrain.Editor
{
    public abstract class TerrainFieldEditor<TTerrain> : NodePathEditor where TTerrain : TerrainField
    {
        private TerrainBuilder<TTerrain> _builder;

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

        public MeshFilter TerrainMeshFilter
        {
            get
            {
                return TerrainField.GetComponent<MeshFilter>();
            }
        }

        public TerrainFieldEditor(TerrainBuilder<TTerrain> builder)
        {
            base.InputHandler.Add(new DuplicateEventCombination(CustomDuplicate));
            _builder = builder;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUI.changed)
            {
                if (TerrainMeshRenderer.sharedMaterial != null)
                {
                    if (TerrainField.shader == null)
                    {
                        TerrainMeshRenderer.material = null;
                    }
                    else if (TerrainMeshRenderer.sharedMaterial.shader != TerrainField.shader)
                    {
                        TerrainMeshRenderer.material = new Material(TerrainField.shader);
                    }
                }
                else if (TerrainField.shader != null)
                {
                    TerrainMeshRenderer.material = new Material(TerrainField.shader);
                }
                BuildTerrain();
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

        protected override void OnNodePathAdded()
        {
            BuildTerrain();
        }

        protected override void NodePathChanged()
        {
            BuildTerrain();
        }

        protected override void OnNodePathRemoved()
        {
            BuildTerrain();
        }

        protected void OnSceneGUIImplementation()
        {
            DrawNodePathEditors();
            DrawCollider();
            HandleInput();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }

        private void Awake()
        {
            BuildTerrain();
        }

        private void CustomDuplicate()
        {
            var clone = Instantiate(this.TerrainField);

            clone.transform.position = this.TerrainField.transform.position;
            clone.transform.rotation = this.TerrainField.transform.rotation;
            clone.transform.localScale = this.TerrainField.transform.lossyScale;

            clone.mesh = new Mesh();
            clone.GetComponent<MeshRenderer>().material = new Material(TerrainMeshRenderer.sharedMaterial);
            clone.transform.parent = this.TerrainField.transform.parent;
        }

        private void BuildTerrain()
        {
            _builder.BuildMeshFor(TerrainField);
            TerrainColliderBuilder.BuildColliderFor(TerrainField);
        }
    }
}
