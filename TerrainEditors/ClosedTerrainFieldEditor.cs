using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terrain;
using UnityEditor;

namespace TerrainEditors
{
    [CustomEditor(typeof(ClosedTerrainField))]
    public class ClosedTerrainFieldEditor : TerrainFieldEditor<ClosedTerrainField>
    {
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
    }
}
