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
    public class TerrainBuilderHelper : ITerrainBuilderHelper, IFloorSegmentBuilder, ISlopeSegmentBuilder, ICeilingSegmentBuilder
    {
        private float _floorHeight;
        private float _slopeHeight;
        private float _ceilingHeight;
        private float _cornerWidth;
        private BuilderDataContext _dataContext;

        private FloorSegmentBuilder _floorBuilder;
        private SlopeSegmentBuilder _slopeBuilder;
        private CeilingSegmentBuilder _ceilingBuilder;

        public Vector3[] Vertices
        {
            get { return _dataContext.Vertices.Select(v => v.ToVector3()).ToArray(); }
        }

        public int[] Indices
        {
            get { return _dataContext.Indices.ToArray(); }
        }

        public Vector2[] Uvs
        {
            get { return _dataContext.Uvs.ToArray(); }
        }

        public Color[] Colors
        {
            get { return _dataContext.Colors.ToArray(); }
        }

        public static ITerrainBuilderHelper GetNewBuilder(
            float floorHeight = 0.5f,
            float slopeHeight = 0.5f,
            float ceilingHeight = 0.5f,
            float cornerWidth = 0.5f)
        {
            return new TerrainBuilderHelper(floorHeight, slopeHeight, ceilingHeight, cornerWidth);
        }

        private TerrainBuilderHelper(
            float floorHeight,
            float slopeHeight,
            float ceilingHeight, 
            float cornerWidth)
        {
            _floorHeight = floorHeight;
            _slopeHeight = slopeHeight;
            _ceilingHeight = ceilingHeight;
            _cornerWidth = cornerWidth;
            _dataContext = new BuilderDataContext();
            _floorBuilder = new FloorSegmentBuilder(_dataContext, _floorHeight, _cornerWidth);
            _slopeBuilder = new SlopeSegmentBuilder(_dataContext, _slopeHeight, _cornerWidth);
            _ceilingBuilder = new CeilingSegmentBuilder(_dataContext, _ceilingHeight, _cornerWidth);
        }

        #region ITerrainBuilderHelper

        public IFloorSegmentBuilder AddFloorSegmentStart(LineSegment2D segment)
        {
            _floorBuilder.AddSegmentStartingCorner(segment.P1, segment.GetOrientationInRadians());
            _floorBuilder.AddFirstSegment(segment);
            return this;
        }

        public ISlopeSegmentBuilder AddSlopeSegmentStart(LineSegment2D segment)
        {
            _slopeBuilder.AddSegmentStartingCorner(segment.P1, segment.GetOrientationInRadians());
            _slopeBuilder.AddFirstSegment(segment);
            return this;
        }

        public ICeilingSegmentBuilder AddCeilingSegmentStart(LineSegment2D segment)
        {
            _ceilingBuilder.AddSegmentStartingCorner(segment.P1, segment.GetOrientationInRadians());
            _ceilingBuilder.AddFirstSegment(segment);
            return this;
        }

        public ITerrainBuilderHelper AddFilling(IEnumerable<LineSegment2D> segments, float fillingLowPoint, float fillingUFactor, float fillingVFactor)
        {
            //Print(segments);
            var segmentArray = segments.ToArray();
            var fillingIntervals = TerrainFillingUtils.GetFillingIntervals(segmentArray, fillingLowPoint);
            foreach (var interval in fillingIntervals)
            {
                AddFillingForInterval(interval, segments, fillingLowPoint, fillingUFactor, fillingVFactor);
            }
            return this;
        }

        #endregion

        #region IFloorSegmentBuilder

        public IFloorSegmentBuilder AddFloorSegment(LineSegment2D segment)
        {
            _floorBuilder.AddNextSegment(segment);
            return this;
        }

        public ITerrainBuilderHelper AddFloorSegmentEnd(Vector2 endPoint, float rotationInRadians)
        {
            _floorBuilder.AddSegmentEndingCorner(endPoint, rotationInRadians);
            return this;
        }

        #endregion

        #region ISlopeSegmentBuilder

        public ISlopeSegmentBuilder AddSlopeSegment(LineSegment2D segment)
        {
            _slopeBuilder.AddNextSegment(segment);
            return this;
        }

        public ITerrainBuilderHelper AddSlopeSegmentEnd(Vector2 endPoint, float rotationInRadians)
        {
            _slopeBuilder.AddSegmentEndingCorner(endPoint, rotationInRadians);
            return this;
        }

        #endregion

        #region ICeilingSegmentBuilder

        public ICeilingSegmentBuilder AddCeilingSegment(LineSegment2D segment)
        {
            _ceilingBuilder.AddNextSegment(segment);
            return this;
        }

        public ITerrainBuilderHelper AddCeilingSegmentEnd(Vector2 endPoint, float rotationInRadians)
        {
            _ceilingBuilder.AddSegmentEndingCorner(endPoint, rotationInRadians);
            return this;
        }

        #endregion

        private void AddFillingForInterval(Tuple<int?, int?> interval, IEnumerable<LineSegment2D> segments, float fillingLowPoint, float fillingUFactor, float fillingVFactor)
        {
            var polygonVertices = segments.GetFillingPolygonVertices(interval, fillingLowPoint).ToArray();
            var polygonColors = Enumerable.Range(0, polygonVertices.Length).Select(idx => TerrainColors.FillingColor).ToArray();
            var polygonUvs = 
                polygonVertices
                    .Select(v =>
                    {
                        v.x *= fillingUFactor;
                        v.y *= fillingVFactor;
                        return v;
                    }).ToArray();
            var currentIndice = _dataContext.CurrentIndice;
            var indices = Triangulator.TriangulatePolygon(polygonVertices.ToArray()).Select(i => i + currentIndice + 1).ToArray();
            _dataContext.Vertices.AddRange(polygonVertices);
            _dataContext.Indices.AddRange(indices);
            _dataContext.Colors.AddRange(polygonColors);
            _dataContext.Uvs.AddRange(polygonUvs);
        }

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
