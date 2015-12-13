using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Terrain.Builder.Helper.Interfaces
{
    public interface ISegmentBuilder
    {
        ISegmentBuilder AddSegment(LineSegment2D segment);
        ITerrainBuilderHelper AddSegmentEnd(Vector2 endPoint, float rotationInRadians);
    }
}
