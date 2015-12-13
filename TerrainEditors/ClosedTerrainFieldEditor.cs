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
        protected override void NodePathChanged()
        {
            throw new NotImplementedException();
        }

        protected override void OnNodePathAdded()
        {
            throw new NotImplementedException();
        }

        protected override void OnNodePathRemoved()
        {
            throw new NotImplementedException();
        }
    }
}
