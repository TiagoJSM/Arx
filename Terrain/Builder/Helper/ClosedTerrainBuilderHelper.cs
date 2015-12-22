using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terrain.Builder.Helper.Interfaces;
using UnityEngine;

namespace Terrain.Builder.Helper
{
    public class ClosedTerrainBuilderHelper :
        TerrainBuilderHelper,
        ITerrainBuilderHelper,
        IFloorSegmentBuilder,
        ISlopeSegmentBuilder,
        ICeilingSegmentBuilder
    {
        public static ITerrainBuilderHelper GetNewBuilder(
            float floorHeight,
            float slopeHeight,
            float ceilingHeight,
            float cornerWidth,
            float fillingUFactor,
            float fillingVFactor)
        {
            return new ClosedTerrainBuilderHelper(floorHeight, slopeHeight, ceilingHeight, cornerWidth, fillingUFactor, fillingVFactor);
        }

        public ClosedTerrainBuilderHelper(
            float floorHeight,
            float slopeHeight,
            float ceilingHeight,
            float cornerWidth,
            float fillingUFactor,
            float fillingVFactor)
            : base(
                floorHeight,
                slopeHeight,
                ceilingHeight,
                cornerWidth,
                0,
                fillingUFactor,
                fillingVFactor)
        {
        }

        public override ITerrainBuilderHelper AddFilling(IEnumerable<LineSegment2D> segments)
        {
            var points = segments.Select(s => s.P2);
            FillingBuilder.AddClosedFilling(points);
            return this;
        }
    }
}
