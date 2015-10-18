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

        public NodeMesh NodeMesh
        {
            get
            {
                return target as NodeMesh;
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

            DrawNodePathEditors();
            HandleInput();

            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

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
            NodeMeshBuilder.BuildMeshFor(NodeMesh);
        }
    }
}
