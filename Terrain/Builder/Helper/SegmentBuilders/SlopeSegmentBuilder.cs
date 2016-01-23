using GenericComponents.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terrain.Builder.Helper.SegmentBuilders
{
    public class SlopeSegmentBuilder : SegmentBuilder
    {
        public SlopeSegmentBuilder(BuilderDataContext context, float height, float cornerWidth)
            :base(context, height, cornerWidth, TerrainColors.SlopeColor, TerrainColors.SlopeEndingsColor)
        {

        }
    }
}
