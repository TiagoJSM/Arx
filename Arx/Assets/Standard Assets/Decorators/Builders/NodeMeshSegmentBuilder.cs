using Assets.Standard_Assets.GenericComponents.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.Decorators.Builders
{
    public class NodeMeshSegmentBuilder : SegmentBuilder
    {
        public NodeMeshSegmentBuilder(BuilderDataContext dataContext, float segmentHeight)
            : base(dataContext, segmentHeight, 0)
        {

        }
    }
}
