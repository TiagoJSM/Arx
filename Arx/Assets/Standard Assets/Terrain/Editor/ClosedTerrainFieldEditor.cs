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
        public ClosedTerrainFieldEditor() : base(new ClosedTerrainBuilder())
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
