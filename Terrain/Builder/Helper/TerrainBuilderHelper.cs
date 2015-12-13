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

        private float _floorHeight;
        private float _slopeHeight;
        private float _ceilingHeight;

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

        public static ITerrainBuilderHelper GetNewBuilder(
            float floorHeight = 0.5f,
            float slopeHeight = 0.5f,
            float ceilingHeight = 0.5f,
            float cornerWidth = 0.5f)
        {
            return new TerrainBuilderHelper(floorHeight, slopeHeight, ceilingHeight, cornerWidth);
        }

        private Vector2 PreviousTopRightCorner
        {
            set
            {
                _vertices[_vertices.Count() - 1] = value;
            }
            get
            {
                return _vertices[_vertices.Count() - 1];
            }
        }

        private TerrainBuilderHelper(
            float floorHeight,
            float slopeHeight,
            float ceilingHeight, 
            float cornerWidth)
        {
            _currentIndice = -1;
            _processedSegments = new List<LineSegment2D>();
            _vertices = new List<Vector2>();
            _indices = new List<int>();
            _uvs = new List<Vector2>();
            _colors = new List<Color>();
            _floorHeight = floorHeight;
            _slopeHeight = slopeHeight;
            _ceilingHeight = ceilingHeight;
            _cornerWidth = cornerWidth;
        }

        #region IFloorSegmentBuilder

        #endregion

        #region ISlopeSegmentBuilder

        #endregion

        public IFloorSegmentBuilder AddFloorSegmentStart(LineSegment2D segment)
        {
            AddSegmentStart(segment, FloorEndingsColor, FloorColor, _floorHeight);
            return this;
        }

        public ISlopeSegmentBuilder AddSlopeSegmentStart(LineSegment2D segment)
        {
            AddSegmentStart(segment, SlopeEndingsColor, SlopeColor, _slopeHeight);
            return this;
        }

        public ICeilingSegmentBuilder AddCeilingSegmentStart(LineSegment2D segment)
        {
            AddSegmentStart(segment, CeilingEndingsColor, CeilingColor, _ceilingHeight);
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

        public IFloorSegmentBuilder AddFloorSegment(LineSegment2D segment)
        {
            AddNextSegment(segment, FloorColor, _floorHeight);
            return this;
        }

        public ITerrainBuilderHelper AddFloorSegmentEnd(Vector2 endPoint, float rotationInRadians)
        {
            AddSegmentCornerEnd(endPoint, rotationInRadians, FloorEndingsColor, _floorHeight);
            return this;
        }

        public ISlopeSegmentBuilder AddSlopeSegment(LineSegment2D segment)
        {
            AddNextSegment(segment, SlopeColor, _slopeHeight);
            return this;
        }

        public ITerrainBuilderHelper AddSlopeSegmentEnd(Vector2 endPoint, float rotationInRadians)
        {
            AddSegmentCornerEnd(endPoint, rotationInRadians, SlopeEndingsColor, _slopeHeight);
            return this;
        }

        public ICeilingSegmentBuilder AddCeilingSegment(LineSegment2D segment)
        {
            AddNextSegment(segment, CeilingColor, _ceilingHeight);
            return this;
        }

        public ITerrainBuilderHelper AddCeilingSegmentEnd(Vector2 endPoint, float rotationInRadians)
        {
            AddSegmentCornerEnd(endPoint, rotationInRadians, CeilingEndingsColor, _ceilingHeight);
            return this;
        }

        private void AddSegmentStart(LineSegment2D segment, Color endingColor, Color segmentColor, float height)
        {
            _currentSegmentIndex = 0;
            AddSegmentCornerStart(segment.P1, segment.GetOrientationInRadians(), endingColor, height);
            AddFirstSegmentStart(segment, segmentColor, height);
        }

        private void AddNextSegment(LineSegment2D segment, Color segmentColor, float height)
        {
            _currentSegmentIndex++;
            ArrangePreviousTopRightCorner(segment.GetOrientationInRadians(), height);
            AddNextSegmentData(segment, segmentColor, height);
        }

        private void AddSegmentCornerStart(Vector2 origin, float rotationInRadians, Color color, float height)
        {
            var vectors = 
                new[] {
                    origin + new Vector2(-_cornerWidth, -height/2),
                    origin + new Vector2(0, -height/2),
                    origin + new Vector2(-_cornerWidth, height/2),
                    origin + new Vector2(0, height/2)
                };

            AddSegmentDataStart(color, false, GetRotatedVectors(origin, rotationInRadians, vectors));
        }

        private void AddSegmentCornerEnd(Vector2 endPoint, float rotationInRadians, Color color, float height)
        {
            var vectors =
                new[] {
                    endPoint + new Vector2(0, -height/2),
                    endPoint + new Vector2(_cornerWidth, -height/2),
                    endPoint + new Vector2(0, height/2),
                    endPoint + new Vector2(_cornerWidth, height/2)
                };

            AddSegmentDataStart(color, true, GetRotatedVectors(endPoint, rotationInRadians, vectors));
        }

        private void AddFirstSegmentStart(LineSegment2D segment, Color color, float height)
        {
            var radians = segment.GetOrientationInRadians();
            var bottomLeft = (segment.P1 - new Vector2(0, height / 2)).RotateAround(segment.P1, radians);
            var bottomRight = (segment.P2 - new Vector2(0, height / 2)).RotateAround(segment.P2, radians);
            var topLeft = (segment.P1 + new Vector2(0, height / 2)).RotateAround(segment.P1, radians);
            var topRight = (segment.P2 + new Vector2(0, height / 2)).RotateAround(segment.P2, radians);

            var v = new []{ bottomLeft, bottomRight, topLeft, topRight };
            //Print(v);
            AddSegmentDataStart(color, false, v);
            _processedSegments.Add(segment);
        }

        private void AddSegmentDataStart(Color color, bool mirrowedUvs, params Vector2[] vertices)
        {
            _vertices.AddRange(vertices);

            var i = new[] {
                    _currentIndice + 1, _currentIndice + 4, _currentIndice + 2,
                    _currentIndice + 1, _currentIndice + 3, _currentIndice + 4
                };

            _indices.AddRange(i);
            //Print(i);

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

        private void AddNextSegmentData(LineSegment2D segment, Color color, float height)
        {
            _processedSegments.Add(segment);

            var radians = segment.GetOrientationInRadians();

            _vertices.AddRange(
                new []
                {
                    (segment.P2 - new Vector2(0, height/2)).RotateAround(segment.P2, radians), 
                    (segment.P2 + new Vector2(0, height/2)).RotateAround(segment.P2, radians)
                });

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
                    new Vector2(_currentSegmentIndex + 1.0f, 0),
                    new Vector2(_currentSegmentIndex + 1.0f, 1),
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

        private void ArrangePreviousTopRightCorner(float rotationInRadians, float height)
        {
            var lastProcessedSegment = _processedSegments.Last();
            var lastSegmentOrientation = lastProcessedSegment.GetOrientationInRadians();
            var radians = (rotationInRadians + lastSegmentOrientation) / 2;
            var bottomRight = lastProcessedSegment.P2;

            if ((Mathf.Abs(rotationInRadians - lastSegmentOrientation)).NormalizeRadians() > Mathf.PI)
            {
                radians -= Mathf.PI;
            }

            PreviousTopRightCorner = (bottomRight + new Vector2(0, height / 2)).RotateAround(bottomRight, radians);
        }

        private void AddFillingForInterval(Tuple<int?, int?> interval, IEnumerable<LineSegment2D> segments, float fillingLowPoint, float fillingUFactor, float fillingVFactor)
        {
            var polygonVertices = segments.GetFillingPolygonVertices(interval, fillingLowPoint).ToArray();
            var polygonColors = Enumerable.Range(0, polygonVertices.Length).Select(idx => FillingColor).ToArray();
            var polygonUvs = 
                polygonVertices
                    .Select(v =>
                    {
                        v.x *= fillingUFactor;
                        v.y *= fillingVFactor;
                        return v;
                    }).ToArray();
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
