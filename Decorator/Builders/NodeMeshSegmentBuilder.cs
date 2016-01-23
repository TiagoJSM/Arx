using GenericComponents.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decorator.Builders
{
    public class NodeMeshSegmentBuilder : SegmentBuilder
    {
        public NodeMeshSegmentBuilder(BuilderDataContext dataContext, float segmentHeight)
            : base(dataContext, segmentHeight, 0)
        {

        }
    }
}
