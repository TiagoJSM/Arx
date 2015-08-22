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
    public class TerrainBuilderHelper : ITerrainBuilderHelper, IFloorSegmentBuilder, ISlopeSegmentBuilder, ICeilingSegmentBuilder
    {
        private readonly Color FloorEndingsColor = new Color(0, 0, 0, 0.0f);
        private readonly Color FloorColor = new Color(0, 0, 0, 0.1f);
        private readonly Color SlopeEndingsColor = new Color(0, 0, 0, 0.2f);
        private readonly Color SlopeColor = new Color(0, 0, 0, 0.3f);
        private readonly Color FillingColor = new Color(0, 0, 0, 0.4f);
        private readonly Color CeilingEndingsColor = new Color(0, 0, 0, 0.5f);
        private readonly Color CeilingColor = new Color(0, 0, 0, 0.6f);

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
        private float _cornerWidth;

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

        public static ITerrainBuilderHelper GetNewBuilder(float height = 0.5f, float cornerWidth = 0.5f)
        {
            return new TerrainBuilderHelper(height, cornerWidth);
        }

        private Vector2 PreviousTopRightCorner
        {
            set
            {
                _vertices[_vertices.Count() - 1] = value;
            }
        }

        private TerrainBuilderHelper(float height, float cornerWidth)
        {
            _currentIndice = -1;
            _processedSegments = new List<LineSegment2D>();
            _vertices = new List<Vector2>();
            _indices = new List<int>();
            _uvs = new List<Vector2>();
            _colors = new List<Color>();
            _height = height;
            _cornerWidth = cornerWidth;
        }

        public IFloorSegmentBuilder AddFloorSegmentStart(LineSegment2D segment)
        {
            AddSegmentStart(segment, FloorEndingsColor, FloorColor);
            return this;
        }

        public ISlopeSegmentBuilder AddSlopeSegmentStart(LineSegment2D segment)
        {
            AddSegmentStart(segment, SlopeEndingsColor, SlopeColor);
            return this;
        }

        public ICeilingSegmentBuilder AddCeilingSegmentStart(LineSegment2D segment)
        {
            AddSegmentStart(segment, CeilingEndingsColor, CeilingColor);
            return this;
        }

        public ITerrainBuilderHelper AddFilling(IEnumerable<LineSegment2D> segments, float fillingLowPoint)
        {
            //Print(segments);
            var segmentArray = segments.ToArray();
            var fillingIntervals = TerrainFillingUtils.GetFillingIntervals(segmentArray, fillingLowPoint);
            foreach (var interval in fillingIntervals)
            {
                AddFillingForInterval(interval, segments, fillingLowPoint);
            }
            return this;
        }

        public IFloorSegmentBuilder AddFloorSegment(LineSegment2D segment)
        {
            AddNextSegment(segment, FloorColor);
            return this;
        }

        public ITerrainBuilderHelper AddFloorSegmentEnd(Vector2 endPoint, float rotationInRadians)
        {
            AddSegmentCornerEnd(endPoint, rotationInRadians, FloorEndingsColor);
            return this;
        }

        public ISlopeSegmentBuilder AddSlopeSegment(LineSegment2D segment)
        {
            AddNextSegment(segment, SlopeColor);
            return this;
        }

        public ITerrainBuilderHelper AddSlopeSegmentEnd(Vector2 endPoint, float rotationInRadians)
        {
            AddSegmentCornerEnd(endPoint, rotationInRadians, SlopeEndingsColor);
            return this;
        }

        public ICeilingSegmentBuilder AddCeilingSegment(LineSegment2D segment)
        {
            AddNextSegment(segment, CeilingColor);
            return this;
        }

        public ITerrainBuilderHelper AddCeilingSegmentEnd(Vector2 endPoint, float rotationInRadians)
        {
            AddSegmentCornerEnd(endPoint, rotationInRadians, CeilingEndingsColor);
            return this;
        }

        private void AddSegmentStart(LineSegment2D segment, Color endingColor, Color segmentColor)
        {
            _currentSegmentIndex = 0;
            AddSegmentCornerStart(segment.P1, segment.GetOrientationInRadians(), endingColor);
            AddFirstSegmentStart(segment, segmentColor);
        }

        private void AddNextSegment(LineSegment2D segment, Color segmentColor)
        {
            _currentSegmentIndex++;
            ArrangePreviousTopRightCorner(segment.GetOrientationInRadians());
            AddNextSegmentData(segment, segmentColor);
        }

        private void AddSegmentCornerStart(Vector2 origin, float rotationInRadians, Color color)
        {
            var vectors = 
                new[] {
                    origin + new Vector2(-_cornerWidth, 0),
                    origin,
                    origin + new Vector2(-_cornerWidth, _height),
                    origin + new Vector2(0, _height)
                };

            AddSegmentDataStart(color, true, GetRotatedVectors(origin, rotationInRadians, vectors));
        }

        private void AddSegmentCornerEnd(Vector2 endPoint, float rotationInRadians, Color color)
        {
            var vectors =
                new[] {
                    endPoint,
                    endPoint + new Vector2(_cornerWidth, 0),
                    endPoint + new Vector2(0, _height),
                    endPoint + new Vector2(_cornerWidth, _height)
                };

            AddSegmentDataStart(color, false, GetRotatedVectors(endPoint, rotationInRadians, vectors));
        }

        private void AddFirstSegmentStart(LineSegment2D segment, Color color)
        {
            var radians = segment.GetOrientationInRadians();
            var bottomLeft = segment.P1;
            var bottomRight = segment.P2;
            var topLeft = (segment.P1 + new Vector2(0, _height)).RotateAround(segment.P1, radians);
            var topRight = (segment.P2 + new Vector2(0, _height)).RotateAround(segment.P2, radians);

            AddSegmentDataStart(color, false, bottomLeft, bottomRight, topLeft, topRight);
            _processedSegments.Add(segment);
        }

        private void AddSegmentDataStart(Color color, bool mirrowedUvs, params Vector2[] vertices)
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

        private void AddNextSegmentData(LineSegment2D segment, Color color)
        {
            _processedSegments.Add(segment);

            var radians = segment.GetOrientationInRadians();

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

        private Vector2[] GetRotatedVectors(Vector2 originPoint, float rotationInRadians, params Vector2[] vectors)
        {
            return vectors.Select(v => v.RotateAround(originPoint, rotationInRadians)).ToArray();
        }

        private void ArrangePreviousTopRightCorner(float rotationInRadians)
        {
            var lastProcessedSegment = _processedSegments.Last();
            var radians = (rotationInRadians + lastProcessedSegment.GetOrientationInRadians()) / 2;
            var bottomRight = lastProcessedSegment.P2;
            PreviousTopRightCorner = (bottomRight + new Vector2(0, _height)).RotateAround(bottomRight, radians);
        }

        private void AddFillingForInterval(Tuple<int?, int?> interval, IEnumerable<LineSegment2D> segments, float fillingLowPoint)
        {
            var polygonVertices = segments.GetFillingPolygonVertices(interval, fillingLowPoint).ToArray();
            var polygonColors = Enumerable.Range(0, polygonVertices.Length).Select(idx => FillingColor).ToArray();
            var polygonUvs = polygonVertices.Select(v => v).ToArray();
            var indices = Triangulator.TriangulatePolygon(polygonVertices.ToArray()).Select(i => i + _currentIndice + 1).ToArray();
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
