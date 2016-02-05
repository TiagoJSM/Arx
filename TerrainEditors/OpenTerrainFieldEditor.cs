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
    [CustomEditor(typeof(OpenTerrainField))]
    public class OpenTerrainFieldEditor : TerrainFieldEditor<OpenTerrainField>
    {
        private OpenTerrainBuilder _builder;

        public OpenTerrainFieldEditor()
        {
            _builder = new OpenTerrainBuilder();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (GUI.changed)
            {
                RequiresMeshUpdate = true;
            }
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
            if (RequiresMeshUpdate)
            {
                RequiresMeshUpdate = false;
                _builder.BuildMeshFor(TerrainField);
                TerrainColliderBuilder.BuildColliderFor(TerrainField);
            }
            DrawNodePathEditors();
            DrawCollider();
            HandleInput();        

            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }
    }
}
