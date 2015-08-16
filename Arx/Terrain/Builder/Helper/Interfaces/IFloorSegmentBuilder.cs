using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Terrain.Builder.Helper.Interfaces
{
    public interface IFloorSegmentBuilder
    {
        IFloorSegmentBuilder AddFloorSegment(LineSegment2D segment);
        ITerrainBuilderHelper AddFloorSegmentEnd(Vector2 endPoint, float rotationInRadians);
    }
}
