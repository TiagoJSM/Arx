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

namespace Terrain.Builder.Helper
{
    public class TerrainBuilderHelper : ITerrainBuilderHelper, IFloorSegmentBuilder, ISlopeSegmentBuilder
    {
        private readonly Color FloorEndingsColor = new Color(0, 0, 0, 0.0f);
        private readonly Color FloorColor = new Color(0, 0, 0, 0.1f);
        private readonly Color SlopeEndingsColor = new Color(0, 0, 0, 0.2f);
        private readonly Color SlopeColor = new Color(0, 0, 0, 0.3f);
        private readonly Color FillingColor = new Color(0, 0, 0, 0.4f);

        private readonly Vector2[] SegmentStartUvs =
            new[]{
                new Vector2(),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };

        private readonly Vector2[] SegmentStartMirroredUvs =
            new[]{
                new Vector2(1, 0),
                new Vector2(),
                new Vector2(1, 1),
                new Vector2(0, 1)
            };

        private float _height;

        private int _currentIndice;
        private int _currentSegmentIndex;

        private List<LineSegment2D> _processedSegments;

        private List<Vector2> _vertices;
        private List<int> _indices;
        private List<Vector2> _uvs;
        private List<Color> _colors;

        public Vector3[] Vertices
        {
            get { return _vertices.Select(v => v.ToVector3()).ToArray(); }
        }

        public int[] Indices
        {
            get { return _indices.ToArray(); }
        }

        public Vector2[] Uvs
        {
            get { return _uvs.ToArray(); }
        }

        public Color[] Colors
        {
            get { return _colors.ToArray(); }
        }

        public static ITerrainBuilderHelper GetNewBuilder()
        {
            return new TerrainBuilderHelper();
        }

        private Vector2 PreviousTopRightCorner
        {
            set
            {
                _vertices[_vertices.Count() - 1] = value;
            }
        }

        private TerrainBuilderHelper()
        {
            _currentIndice = -1;
            _processedSegments = new List<LineSegment2D>();
            _vertices = new List<Vector2>();
            _indices = new List<int>();
            _uvs = new List<Vector2>();
            _colors = new List<Color>();

            _height = 0.5f;
        }

        public IFloorSegmentBuilder AddFloorSegmentStart(LineSegment2D segment)
        {
            _currentSegmentIndex = 0;
            AddEndingSegmentStart(segment.P1, segment.Slope, FloorEndingsColor);
            AddFirstSegmentStart(segment, segment.Slope, FloorColor);
            return this;
        }

        public ISlopeSegmentBuilder AddSlopeSegmentStart(LineSegment2D segment)
        {
            _currentSegmentIndex = 0;
            AddEndingSegmentStart(segment.P1, segment.Slope, SlopeEndingsColor);
            AddFirstSegmentStart(segment, segment.Slope, SlopeColor);
            return this;
        }

        public ITerrainBuilderHelper AddFilling(IEnumerable<LineSegment2D> segments, float fillingLowPoint)
        {
            var segmentArray = segments.ToArray();
            var fillingIntervals = TerrainFillingUtils.GetFillingIntervals(segmentArray, fillingLowPoint);
            Print(segmentArray);
            foreach (var interval in fillingIntervals)
            {
                AddFillingForInterval(interval, segments, fillingLowPoint);
            }
            return this;
        }

        public IFloorSegmentBuilder AddFloorSegment(LineSegment2D segment)
        {
            _currentSegmentIndex++;
            ArrangePreviousTopRightCorner(segment.Slope);
            AddNextSegment(segment, segment.Slope, FloorColor);
            return this;
        }

        public ITerrainBuilderHelper AddFloorSegmentEnd(Vector2 endPoint, float? slope)
        {
            AddEndingSegmentEnd(endPoint, slope, FloorEndingsColor);
            return this;
        }

        public ISlopeSegmentBuilder AddSlopeSegment(LineSegment2D segment)
        {
            _currentSegmentIndex++;
            ArrangePreviousTopRightCorner(segment.Slope);
            AddNextSegment(segment, segment.Slope, SlopeColor);
            return this;
        }

        public ITerrainBuilderHelper AddSlopeSegmentEnd(Vector2 endPoint, float? slope)
        {
            AddEndingSegmentEnd(endPoint, slope, SlopeEndingsColor);
            return this;
        }

        private void AddEndingSegmentStart(Vector2 origin, float? slope, Color color)
        {
            var vectors = 
                new[] {
                    origin + new Vector2(-_height, 0),
                    origin,
                    origin + new Vector2(-_height, _height),
                    origin + new Vector2(0, _height)
                };

            AddSegmentStart(color, true, GetRotatedVectors(origin, slope, vectors));
        }

        private void AddEndingSegmentEnd(Vector2 endPoint, float? slope, Color color)
        {
            var vectors =
                new[] {
                    endPoint,
                    endPoint + new Vector2(_height, 0),
                    endPoint + new Vector2(0, _height),
                    endPoint + new Vector2(_height, _height)
                };

            AddSegmentStart(color, false, GetRotatedVectors(endPoint, slope, vectors));
        }

        private void AddFirstSegmentStart(LineSegment2D segment, float? slope, Color color)
        {
            var radians = Mathf.Atan(slope.Value);
            var bottomLeft = segment.P1;
            var bottomRight = segment.P2;
            var topLeft = (segment.P1 + new Vector2(0, _height)).RotateAround(segment.P1, radians);
            var topRight = (segment.P2 + new Vector2(0, _height)).RotateAround(segment.P2, radians);

            AddSegmentStart(color, false, bottomLeft, bottomRight, topLeft, topRight);
            _processedSegments.Add(segment);
        }

        private void AddSegmentStart(Color color, bool mirrowedUvs, params Vector2[] vertices)
        {
            _vertices.AddRange(vertices);

            _indices.AddRange(
                new[] { 
                    _currentIndice + 1, _currentIndice + 4, _currentIndice + 2,
                    _currentIndice + 1, _currentIndice + 3, _currentIndice + 4
                });

            _currentIndice += 4;

            if (mirrowedUvs)
            {
                _uvs.AddRange(SegmentStartMirroredUvs);
            }
            else
            {
                _uvs.AddRange(SegmentStartUvs);
            }

            _colors.AddRange(
                new[]{
                    color,
                    color,
                    color,
                    color
                });
        }

        private void AddNextSegment(LineSegment2D segment, float? slope, Color color)
        {
            _processedSegments.Add(segment);

            var radians = Mathf.Atan(slope.Value);

            _vertices.AddRange(new []{segment.P2, (segment.P2 + new Vector2(0, _height)).RotateAround(segment.P2, radians)});

            if(_currentSegmentIndex == 1)
            {
                 _indices.AddRange(
                    new[] { 
                        _currentIndice - 2 , _currentIndice + 2, _currentIndice + 1,
                        _currentIndice - 2, _currentIndice, _currentIndice + 2
                });
            }
            else
            {
                _indices.AddRange(
                    new[] { 
                        _currentIndice - 1 , _currentIndice + 2, _currentIndice + 1,
                        _currentIndice - 1, _currentIndice, _currentIndice + 2
                });
            }

            _currentIndice += 2;

            _uvs.AddRange(
                new[]{
                    new Vector2(_currentSegmentIndex + 1, 0),
                    new Vector2(_currentSegmentIndex + 1, 1),
                });

            _colors.AddRange(
                new[]{
                    color,
                    color
                });
        }

        private Vector2[] GetRotatedVectors(Vector2 originPoint, float? slope, params Vector2[] vectors)
        {
            var radians = Mathf.Atan(slope.Value);
            return vectors.Select(v => v.RotateAround(originPoint, radians)).ToArray();
        }

        private void ArrangePreviousTopRightCorner(float? slope)
        {
            var lastProcessedSegment = _processedSegments.Last();
            var radians = (Mathf.Atan(slope.Value) + Mathf.Atan(lastProcessedSegment.Slope.Value)) / 2;
            var bottomRight = lastProcessedSegment.P2;
            PreviousTopRightCorner = (bottomRight + new Vector2(0, _height)).RotateAround(bottomRight, radians);
        }

        private void AddFillingForInterval(Tuple<int?, int?> interval, IEnumerable<LineSegment2D> segments, float fillingLowPoint)
        {
            var polygonVertices = segments.GetFillingPolygonVertices(interval, fillingLowPoint).ToArray();
            var polygonColors = Enumerable.Range(0, polygonVertices.Length).Select(idx => FillingColor).ToArray();
            var polygonUvs = polygonVertices.Select(v => v).ToArray();
            var indices = Triangulator.TriangulatePolygon(polygonVertices.ToArray()).Select(i => i + _currentIndice + 1).ToArray();
            Print(polygonVertices);
            Print(indices);
            _vertices.AddRange(polygonVertices);
            _indices.AddRange(indices);
            _colors.AddRange(polygonColors);
            _uvs.AddRange(polygonUvs);
            _currentIndice = indices.Max();
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
