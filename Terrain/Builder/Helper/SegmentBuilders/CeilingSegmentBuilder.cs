using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terrain.Builder.Helper.SegmentBuilders
{
    public class CeilingSegmentBuilder : SegmentBuilder
    {
        public CeilingSegmentBuilder(BuilderDataContext context, float height, float cornerWidth)
            :base(context, height, cornerWidth, TerrainColors.CeilingColor, TerrainColors.CeilingEndingsColor)
        {

        }
    }
}
