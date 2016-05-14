using CommonEditors;
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
            base.InputHandler.Add(new DuplicateEventCombination(CustomDuplicate));
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

        private void CustomDuplicate()
        {
            var clone = Instantiate(this.NodeMesh);

            clone.transform.position = this.NodeMesh.transform.position;
            clone.transform.rotation = this.NodeMesh.transform.rotation;
            clone.transform.localScale = this.NodeMesh.transform.lossyScale;

            clone.mesh = new Mesh();
            if (NodeMeshRenderer.sharedMaterial != null)
            {
                clone.GetComponent<MeshRenderer>().material = new Material(NodeMeshRenderer.sharedMaterial);
            }
            clone.transform.parent = this.NodeMesh.transform.parent;
        }
    }
}
