using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terrain;
using Terrain.Builder;
using UnityEditor;
using UnityEngine;

namespace TerrainEditors
{
    [CustomEditor(typeof(ClosedTerrainField))]
    public class ClosedTerrainFieldEditor : TerrainFieldEditor<ClosedTerrainField>
    {
        private ClosedTerrainBuilder _builder;

        public ClosedTerrainFieldEditor()
        {
            _builder = new ClosedTerrainBuilder();
        }

        protected override void OnNodePathAdded()
        {
            RequiresMeshUpdate = true;
        }

        protected override void NodePathChanged()
        {
            RequiresMeshUpdate = true;
        }

        protected override void OnNodePathRemoved()
        {
            RequiresMeshUpdate = true;
        }

        private void OnSceneGUI()
        {
            base.OnSceneGUI();
            if (RequiresMeshUpdate)
            {
                RequiresMeshUpdate = false;
                _builder.BuildMeshFor(TerrainField);
                //TerrainColliderBuilder.BuildColliderFor(TerrainField);
            }
            DrawNodePathEditors();
            DrawCollider();
            HandleInput();

            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }
    }
}
