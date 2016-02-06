using Decorator;
using Decorator.Builders;
using Decorator.NodeMeshDecoration;
using GenericComponentEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace DecoratorEditors.NodeMeshDecoration
{
    [CustomEditor(typeof(NodeMesh))]
    public class NodeMeshEditor : NodePathEditor
    {
        private bool _requiresMeshUpdate;
        private bool _shaderChanged;
        private Shader _previousShader;

        public NodeMesh NodeMesh
        {
            get
            {
                return target as NodeMesh;
            }
        }
        public MeshRenderer NodeMeshRenderer
        {
            get
            {
                return NodeMesh.GetComponent<MeshRenderer>();
            }
        }

        public NodeMeshEditor()
        {
            _requiresMeshUpdate = true;
        }

        private void OnSceneGUI()
        {
            if (_requiresMeshUpdate)
            {
                _requiresMeshUpdate = false;
                NodeMeshBuilder.BuildMeshFor(NodeMesh);
            }
            if (_shaderChanged && NodeMesh.shader != null)
            {
                NodeMeshRenderer.material = new Material(NodeMesh.shader);
            }

            DrawNodePathEditors();
            HandleInput();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (_previousShader != NodeMesh.shader)
            {
                _shaderChanged = true;
                _previousShader = NodeMesh.shader;
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

        protected override void NodePathChanged()
        {
            _requiresMeshUpdate = true;
        }

        protected override void OnNodePathAdded()
        {
            _requiresMeshUpdate = true;
        }

        protected override void OnNodePathRemoved()
        {
            _requiresMeshUpdate = true;
        }
    }
}
