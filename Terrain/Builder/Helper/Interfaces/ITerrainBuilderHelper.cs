using Extensions;
using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Terrain.Builder.Helper.Interfaces
{
    public interface ITerrainBuilderHelper
    {
        Vector3[] Vertices { get; }
        int[] Indices { get; }
        Vector2[] Uvs { get; }
        Color[] Colors { get; }

        IFloorSegmentBuilder AddFloorSegmentStart(LineSegment2D segment);
        ISlopeSegmentBuilder AddSlopeSegmentStart(LineSegment2D segment);
        ICeilingSegmentBuilder AddCeilingSegmentStart(LineSegment2D segment);
        ITerrainBuilderHelper AddFilling(IEnumerable<LineSegment2D> segments);
    }
}
