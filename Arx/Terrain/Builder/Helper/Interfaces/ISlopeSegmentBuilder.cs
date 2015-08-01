using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Terrain.Builder.Helper.Interfaces
{
    public interface ISlopeSegmentBuilder
    {
        ISlopeSegmentBuilder AddSlopeSegment(LineSegment2D segment);
        ITerrainBuilderHelper AddSlopeSegmentEnd(Vector2 endPoint, float? slope);
    }
}
