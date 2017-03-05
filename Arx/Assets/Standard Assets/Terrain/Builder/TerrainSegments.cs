using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.Terrain.Builder
{
    public class TerrainSegments
    {
        public List<LineSegment2D> Segments { get; set; }
        public TerrainType TerrainType { get; set; }

        public TerrainSegments()
        {
            Segments = new List<LineSegment2D>();
        }
    }
}
