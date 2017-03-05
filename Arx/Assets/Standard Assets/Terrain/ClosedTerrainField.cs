using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.Terrain
{
    public class ClosedTerrainField : TerrainField
    {
        public ClosedTerrainField()
        {
            NodePath.IsCircular = true;
        }
    }
}
