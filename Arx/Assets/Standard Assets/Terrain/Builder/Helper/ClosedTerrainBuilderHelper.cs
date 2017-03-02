using Assets.Standard_Assets.Terrain.Builder.Helper.Interfaces;
using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Terrain.Builder.Helper
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
            float floorCornerWidth,
            float slopeCornerWidth,
            float ceilingCornerWidth,
            float fillingUFactor,
            float fillingVFactor)
        {
            return new ClosedTerrainBuilderHelper(floorHeight, slopeHeight, ceilingHeight, floorCornerWidth, slopeCornerWidth, ceilingCornerWidth, fillingUFactor, fillingVFactor);
        }

        public ClosedTerrainBuilderHelper(
            float floorHeight,
            float slopeHeight,
            float ceilingHeight,
            float floorCornerWidth,
            float slopeCornerWidth,
            float ceilingCornerWidth,
            float fillingUFactor,
            float fillingVFactor)
            : base(
                floorHeight,
                slopeHeight,
                ceilingHeight,
                floorCornerWidth,
                slopeCornerWidth,
                ceilingCornerWidth,
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
