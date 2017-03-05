using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using MathHelper.Extensions;
using UnityEngine;
using Assets.Standard_Assets.Terrain.Utils;
using Extensions;
using Assets.Standard_Assets.GenericComponents.Builders;

namespace Assets.Standard_Assets.Terrain.Builder.Helper.SegmentBuilders
{
    public class FillingBuilder
    {
        private BuilderDataContext _dataContext;
        private float _fillingLowPoint;
        private float _fillingUFactor;
        private float _fillingVFactor;

        public FillingBuilder(
            BuilderDataContext dataContext,
            float fillingLowPoint,
            float fillingUFactor,
            float fillingVFactor)
        {
            _dataContext = dataContext;
            _fillingLowPoint = fillingLowPoint;
            _fillingUFactor = fillingUFactor;
            _fillingVFactor = fillingVFactor;
        }

        public void AddClosedFilling(IEnumerable<Vector2> vertices)
        {
            var polygonVertices = vertices.ToArray();
            var polygonColors = Enumerable.Range(0, polygonVertices.Length).Select(idx => TerrainColors.FillingColor).ToArray();
            var polygonUvs =
                polygonVertices
                    .Select(v =>
                    {
                        v.x *= _fillingUFactor;
                        v.y *= _fillingVFactor;
                        return v;
                    }).ToArray();
            var currentIndice = _dataContext.CurrentIndice;
            var indices = Triangulator.TriangulatePolygon(polygonVertices.ToArray()).Select(i => i + currentIndice + 1).ToArray();
            _dataContext.Vertices.AddRange(polygonVertices);
            _dataContext.Indices.AddRange(indices);
            _dataContext.Colors.AddRange(polygonColors);
            _dataContext.Uvs.AddRange(polygonUvs);
        }

        public void AddOpenFilling(IEnumerable<LineSegment2D> segments)
        {
            var segmentArray = segments.ToArray();
            var fillingIntervals = TerrainFillingUtils.GetFillingIntervals(segmentArray, _fillingLowPoint);
            foreach (var interval in fillingIntervals)
            {
                AddFillingForInterval(interval, segments);
            }
        }

        private void AddFillingForInterval(Tuple<int?, int?> interval, IEnumerable<LineSegment2D> segments)
        {
            var polygonVertices = segments.GetFillingPolygonVertices(interval, _fillingLowPoint).ToArray();
            var polygonColors = Enumerable.Range(0, polygonVertices.Length).Select(idx => TerrainColors.FillingColor).ToArray();
            var polygonUvs =
                polygonVertices
                    .Select(v =>
                    {
                        v.x *= _fillingUFactor;
                        v.y *= _fillingVFactor;
                        return v;
                    }).ToArray();
            var currentIndice = _dataContext.CurrentIndice;
            var indices = Triangulator.TriangulatePolygon(polygonVertices.ToArray()).Select(i => i + currentIndice + 1).ToArray();
            _dataContext.Vertices.AddRange(polygonVertices);
            _dataContext.Indices.AddRange(indices);
            _dataContext.Colors.AddRange(polygonColors);
            _dataContext.Uvs.AddRange(polygonUvs);
        }
    }
}
