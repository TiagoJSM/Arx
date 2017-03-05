using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.Terrain.Builder.Helper
{
    public static class TerrainBuilderAnalyzer
    {
        public static TerrainType GetTerrainTypeFromSegment(LineSegment2D segment, float floorTerrainMaximumSlope)
        {
            if (segment.Slope == null || Math.Abs(segment.Slope.Value) >= floorTerrainMaximumSlope)
            {
                return TerrainType.Slope;
            }
            if (segment.NormalVector.y < 0)
            {
                return TerrainType.Ceiling;
            }
            return TerrainType.Floor;
        }
    }
}
