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
            float floorHeight = 0.5f,
            float slopeHeight = 0.5f,
            float ceilingHeight = 0.5f)
        {
            return new ClosedTerrainBuilderHelper(floorHeight, slopeHeight, ceilingHeight);
        }

        public ClosedTerrainBuilderHelper(
            float floorHeight,
            float slopeHeight,
            float ceilingHeight)
            : base(
                floorHeight,
                slopeHeight,
                ceilingHeight,
                0)
        {
        }

        #region ITerrainBuilderHelper

        public IFloorSegmentBuilder AddFloorSegmentStart(LineSegment2D segment)
        {
            FloorBuilder.AddFirstSegment(segment);
            return this;
        }

        public ISlopeSegmentBuilder AddSlopeSegmentStart(LineSegment2D segment)
        {
            SlopeBuilder.AddFirstSegment(segment);
            return this;
        }

        public ICeilingSegmentBuilder AddCeilingSegmentStart(LineSegment2D segment)
        {
            CeilingBuilder.AddFirstSegment(segment);
            return this;
        }

        public ITerrainBuilderHelper AddFilling(IEnumerable<LineSegment2D> segments, float fillingLowPoint, float fillingUFactor, float fillingVFactor)
        {
            //Print(segments);
            /*var segmentArray = segments.ToArray();
            var fillingIntervals = TerrainFillingUtils.GetFillingIntervals(segmentArray, fillingLowPoint);
            foreach (var interval in fillingIntervals)
            {
                AddFillingForInterval(interval, segments, fillingLowPoint, fillingUFactor, fillingVFactor);
            }*/
            return this;
        }

        #endregion

        #region IFloorSegmentBuilder

        public IFloorSegmentBuilder AddFloorSegment(LineSegment2D segment)
        {
            FloorBuilder.AddNextSegment(segment);
            return this;
        }

        public ITerrainBuilderHelper AddFloorSegmentEnd(Vector2 endPoint, float rotationInRadians)
        {
            FloorBuilder.AddSegmentEndingCorner(endPoint, rotationInRadians);
            return this;
        }

        #endregion

        #region ISlopeSegmentBuilder

        public ISlopeSegmentBuilder AddSlopeSegment(LineSegment2D segment)
        {
            SlopeBuilder.AddNextSegment(segment);
            return this;
        }

        public ITerrainBuilderHelper AddSlopeSegmentEnd(Vector2 endPoint, float rotationInRadians)
        {
            SlopeBuilder.AddSegmentEndingCorner(endPoint, rotationInRadians);
            return this;
        }

        #endregion

        #region ICeilingSegmentBuilder

        public ICeilingSegmentBuilder AddCeilingSegment(LineSegment2D segment)
        {
            CeilingBuilder.AddNextSegment(segment);
            return this;
        }

        public ITerrainBuilderHelper AddCeilingSegmentEnd(Vector2 endPoint, float rotationInRadians)
        {
            CeilingBuilder.AddSegmentEndingCorner(endPoint, rotationInRadians);
            return this;
        }

        #endregion
    }
}
