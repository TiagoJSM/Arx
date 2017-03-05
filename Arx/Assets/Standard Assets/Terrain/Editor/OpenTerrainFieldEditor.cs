using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Extensions;
using Utils;
using CommonEditors;
using Assets.Standard_Assets.Terrain.Builder;

namespace Assets.Standard_Assets.Terrain.Editor
{
    [CustomEditor(typeof(OpenTerrainField))]
    public class OpenTerrainFieldEditor : TerrainFieldEditor<OpenTerrainField>
    {
        public OpenTerrainFieldEditor() : base(new OpenTerrainBuilder())
        {
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        private void OnSceneGUI()
        {
            OnSceneGUIImplementation();
        }
    }
}
