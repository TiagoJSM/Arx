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

        //#region ITerrainBuilderHelper

        //public IFloorSegmentBuilder AddFloorSegmentStart(LineSegment2D segment)
        //{
        //    FloorBuilder.AddFirstSegment(segment);
        //    return this;
        //}

        //public ISlopeSegmentBuilder AddSlopeSegmentStart(LineSegment2D segment)
        //{
        //    SlopeBuilder.AddFirstSegment(segment);
        //    return this;
        //}

        //public ICeilingSegmentBuilder AddCeilingSegmentStart(LineSegment2D segment)
        //{
        //    CeilingBuilder.AddFirstSegment(segment);
        //    return this;
        //}

        public override ITerrainBuilderHelper AddFilling(IEnumerable<LineSegment2D> segments)
        {
            var points = segments.Select(s => s.P2);
            FillingBuilder.AddClosedFilling(points);
            return this;
        }

        //#endregion

        //#region IFloorSegmentBuilder

        //public IFloorSegmentBuilder AddFloorSegment(LineSegment2D segment)
        //{
        //    FloorBuilder.AddNextSegment(segment);
        //    return this;
        //}

        //public ITerrainBuilderHelper AddFloorSegmentEnd(Vector2 endPoint, float rotationInRadians)
        //{
        //    FloorBuilder.AddSegmentEndingCorner(endPoint, rotationInRadians);
        //    return this;
        //}

        //#endregion

        //#region ISlopeSegmentBuilder

        //public ISlopeSegmentBuilder AddSlopeSegment(LineSegment2D segment)
        //{
        //    SlopeBuilder.AddNextSegment(segment);
        //    return this;
        //}

        //public ITerrainBuilderHelper AddSlopeSegmentEnd(Vector2 endPoint, float rotationInRadians)
        //{
        //    SlopeBuilder.AddSegmentEndingCorner(endPoint, rotationInRadians);
        //    return this;
        //}

        //#endregion

        //#region ICeilingSegmentBuilder

        //public ICeilingSegmentBuilder AddCeilingSegment(LineSegment2D segment)
        //{
        //    CeilingBuilder.AddNextSegment(segment);
        //    return this;
        //}

        //public ITerrainBuilderHelper AddCeilingSegmentEnd(Vector2 endPoint, float rotationInRadians)
        //{
        //    CeilingBuilder.AddSegmentEndingCorner(endPoint, rotationInRadians);
        //    return this;
        //}

        //#endregion
    }
}
