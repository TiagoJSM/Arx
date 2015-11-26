using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using MathHelper.DataStructures;
using Terrain.Builder.Helper;
using Terrain.Builder.Helper.Interfaces;
using MathHelper.Extensions;

namespace Terrain.Builder
{
    public static class TerrainBuilder
    {
        private static int[] _firstTwoTriangleIndices =
            new[]
            {
                0, 3, 1,
                0, 2, 3
            };
        private static int[] _secondTwoTriangleIndices =
            new[]
            {
                1, 5, 4,
                1, 3, 5
            };

        public static void BuildMeshFor(TerrainField field)
        {
            field.mesh.uv = null;
            field.mesh.triangles = null;
            field.mesh.vertices = null;

            var terrainSegments = GetTerrainSegmentsFor(field);
            var helper = TerrainBuilderHelper.GetNewBuilder(field.terrainFloorHeight, field.terrainSlopeHeight, field.terrainCeilingHeight, field.cornerWidth);
            if (field.addFilling)
            {
                helper = AddFilling(helper, field.PathSegments, field.fillingLowPoint, field.transform.position, field.fillingUFactor, field.fillingVFactor);
            } 
            helper = AddSlopeSegments(helper, terrainSegments);
            helper = AddFloorSegments(helper, terrainSegments);
            helper = AddCeilingSegments(helper, terrainSegments);

            field.mesh.vertices = helper.Vertices;
            field.mesh.triangles = helper.Indices;
            field.mesh.colors = helper.Colors;
            field.mesh.uv = helper.Uvs;

            field.GetComponent<MeshFilter>().mesh = field.mesh;
        }

        private static IEnumerable<TerrainSegments> GetTerrainSegmentsFor(TerrainField field)
        {
            var terrainSegments = new List<TerrainSegments>();
            
            var segments = new TerrainSegments();
            var terrainType = TerrainType.Floor;
            
            foreach (var seg in field.PathSegments)
            {
                var segmentTerrainType = GetTerrainTypeFromSegment(seg, field.floorTerrainMaximumSlope);
                var maxSegmentLenght = GetMaxSegmentLenght(field, segmentTerrainType);
                if (segmentTerrainType != terrainType)
                {
                    if (segments.Segments.Count > 0)
                    {
                        terrainSegments.Add(segments);
                        segments = new TerrainSegments();
                    }
                    terrainType = segmentTerrainType;
                }
                
                segments.TerrainType = segmentTerrainType;
                var dividedSegments = DivideSegment(seg, maxSegmentLenght);
                segments.Segments.AddRange(dividedSegments);
            }

            if (segments.Segments.Count > 0)
            {
                terrainSegments.Add(segments);
            }

            return terrainSegments;
        }

        private static TerrainType GetTerrainTypeFromSegment(LineSegment2D segment, float floorTerrainMaximumSlope)
        {
            if (segment.Slope == null || Math.Abs(segment.Slope.Value) >= floorTerrainMaximumSlope)
            {
                return TerrainType.Slope;
            }
            if (segment.NormalVector.y < 0)
            {
                return TerrainType.Ceiling;
            }
            return TerrainType.Floor;
        }

        private static ITerrainBuilderHelper AddFloorSegments(ITerrainBuilderHelper helper, IEnumerable<TerrainSegments> terrainSegments)
        {
            return
                AddSegments(
                    helper,
                    terrainSegments,
                    TerrainType.Floor,
                    helper.AddFloorSegmentStart,
                    (h, s) => h.AddFloorSegment(s),
                    (h, v, f) => h.AddFloorSegmentEnd(v, f));
        }

        private static ITerrainBuilderHelper AddSlopeSegments(ITerrainBuilderHelper helper, IEnumerable<TerrainSegments> terrainSegments)
        {
            return
                AddSegments(
                    helper,
                    terrainSegments,
                    TerrainType.Slope,
                    helper.AddSlopeSegmentStart,
                    (h, s) => h.AddSlopeSegment(s),
                    (h, v, f)=> h.AddSlopeSegmentEnd(v, f));
        }

        private static ITerrainBuilderHelper AddCeilingSegments(ITerrainBuilderHelper helper, IEnumerable<TerrainSegments> terrainSegments)
        {
            return
                AddSegments(
                    helper,
                    terrainSegments,
                    TerrainType.Ceiling,
                    helper.AddCeilingSegmentStart,
                    (h, s) => h.AddCeilingSegment(s),
                    (h, v, f) => h.AddCeilingSegmentEnd(v, f));
        }

        private static ITerrainBuilderHelper AddSegments<TBuilder>(
            ITerrainBuilderHelper helper, 
            IEnumerable<TerrainSegments> terrainSegments, 
            TerrainType terrainType,
            Func<LineSegment2D, TBuilder> addSegmentStart,
            Func<TBuilder, LineSegment2D, TBuilder> addSegment,
            Func<TBuilder, Vector2, float, ITerrainBuilderHelper> addSegmentEnd)
            where TBuilder : class
        {
            var typeSegments = terrainSegments.Where(s => s.TerrainType == terrainType);
            foreach (var typeSegment in typeSegments)
            {
                TBuilder tBuilder = null;
                typeSegment.Segments.ForEach(s =>
                {
                    if (tBuilder == null)
                    {
                        tBuilder = addSegmentStart(s);
                    }
                    else
                    {
                        tBuilder = addSegment(tBuilder, s);
                    }
                });
                var lastSlopeSegment = typeSegment.Segments.Last();
                helper = addSegmentEnd(tBuilder, lastSlopeSegment.P2, lastSlopeSegment.GetOrientationInRadians());
            }
            return helper;
        }

        private static ITerrainBuilderHelper AddFilling(ITerrainBuilderHelper helper, IEnumerable<LineSegment2D> segments, float fillingLowPoint, Vector3 terrainPosition, float fillingUFactor, float fillingVFactor)
        {
            return helper.AddFilling(segments, fillingLowPoint - terrainPosition.y, fillingUFactor, fillingVFactor);
        }

        private static IEnumerable<LineSegment2D> DivideSegment(LineSegment2D seg, float maxSegmentLenght)
        {   
            if (seg.Lenght <= maxSegmentLenght)
            {
                return new[] { seg };
            }

            var numberOfDivisions = Mathf.CeilToInt(seg.Lenght / maxSegmentLenght);

            var p1 = seg.P1;
            var sizeOfEachPart = (seg.P2 - seg.P1) / numberOfDivisions;
            var result = new LineSegment2D[numberOfDivisions];
            for (var idx = 0; idx < numberOfDivisions; idx++)
            {
                result[idx] = new LineSegment2D(p1, p1 + sizeOfEachPart);
                p1 = p1 + sizeOfEachPart;
            }
            return result;
        }

        private static float GetMaxSegmentLenght(TerrainField field, TerrainType segmentTerrainType)
        {
            switch (segmentTerrainType)
            {
                case TerrainType.Ceiling:
                    return field.maxCeilingSegmentLenght;
                case TerrainType.Floor:
                    return field.maxFloorSegmentLenght;
                case TerrainType.Slope:
                    return field.maxSlopeSegmentLenght;
                default:
                    return field.maxFloorSegmentLenght;
            }
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
