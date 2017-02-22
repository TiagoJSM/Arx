using Assets.Standard_Assets.Terrain.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Standard_Assets.Terrain.Editor
{
    [CustomEditor(typeof(ClosedTerrainField))]
    public class ClosedTerrainFieldEditor : TerrainFieldEditor<ClosedTerrainField>
    {
        private ClosedTerrainBuilder _builder;

        public ClosedTerrainFieldEditor()
        {
            _builder = new ClosedTerrainBuilder();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        protected override void OnNodePathAdded()
        {
        }

        protected override void NodePathChanged()
        {
        }

        protected override void OnNodePathRemoved()
        {
        }

        private void OnSceneGUI()
        {
            _builder.BuildMeshFor(TerrainField);
            TerrainColliderBuilder.BuildColliderFor(TerrainField);
            DrawNodePathEditors();
            DrawCollider();
            HandleInput();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
