using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terrain.Builder.Helper.Interfaces;
using UnityEngine;
using Extensions;
using MathHelper.Extensions;
using Terrain.Utils;
using Utils;
using Terrain.Builder.Helper.SegmentBuilders;

namespace Terrain.Builder.Helper
{
    public class OpenTerrainBuilderHelper : 
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
            float fillingLowPoint, 
            float fillingUFactor,
            float fillingVFactor)
        {
            return new OpenTerrainBuilderHelper(
                floorHeight, 
                slopeHeight, 
                ceilingHeight, 
                cornerWidth, 
                fillingLowPoint, 
                fillingUFactor, 
                fillingVFactor);
        }

        public OpenTerrainBuilderHelper(
            float floorHeight,
            float slopeHeight,
            float ceilingHeight, 
            float cornerWidth,
            float fillingLowPoint,
            float fillingUFactor,
            float fillingVFactor)
            : base(
                floorHeight,
                slopeHeight,
                ceilingHeight,
                cornerWidth,
                fillingLowPoint,
                fillingUFactor,
                fillingVFactor)
        {
        }

        //#region ITerrainBuilderHelper

        //public IFloorSegmentBuilder AddFloorSegmentStart(LineSegment2D segment)
        //{
        //    FloorBuilder.AddSegmentStartingCorner(segment.P1, segment.GetOrientationInRadians());
        //    FloorBuilder.AddFirstSegment(segment);
        //    return this;
        //}

        //public ISlopeSegmentBuilder AddSlopeSegmentStart(LineSegment2D segment)
        //{
        //    SlopeBuilder.AddSegmentStartingCorner(segment.P1, segment.GetOrientationInRadians());
        //    SlopeBuilder.AddFirstSegment(segment);
        //    return this;
        //}

        //public ICeilingSegmentBuilder AddCeilingSegmentStart(LineSegment2D segment)
        //{
        //    CeilingBuilder.AddSegmentStartingCorner(segment.P1, segment.GetOrientationInRadians());
        //    CeilingBuilder.AddFirstSegment(segment);
        //    return this;
        //}

        //public ITerrainBuilderHelper AddFilling(IEnumerable<LineSegment2D> segments)
        //{
        //    FillingBuilder.AddOpenFilling(segments);
        //    return this;
        //}

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

        private static void Print<T>(IEnumerable<T> data)
        {
            string result = string.Empty;
            foreach (var d in data)
            {
                result = result + d.ToString() + ", ";
            }
            Debug.Log(result);
        }
    }
}
