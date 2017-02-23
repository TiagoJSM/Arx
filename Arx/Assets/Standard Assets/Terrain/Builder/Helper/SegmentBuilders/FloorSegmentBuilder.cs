using Assets.Standard_Assets.GenericComponents.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.Terrain.Builder.Helper.SegmentBuilders
{
    public class FloorSegmentBuilder : SegmentBuilder
    {
        public FloorSegmentBuilder(BuilderDataContext context, float height, float cornerWidth)
            :base(context, height, cornerWidth, TerrainColors.FloorColor, TerrainColors.FloorEndingsColor)
        {

        }
    }
}
