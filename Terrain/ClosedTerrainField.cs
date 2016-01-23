using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terrain
{
    public class ClosedTerrainField : TerrainField
    {
        public ClosedTerrainField()
        {
            NodePath.IsCircular = true;
        }
    }
}
