using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using MathHelper.DataStructures;
using Terrain.Builder.Helper;
using Terrain.Builder.Helper.Interfaces;

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
            //mesh.name = "random";
            field.mesh.uv = null;
            field.mesh.triangles = null;
            field.mesh.vertices = null;

            var terrainSegments = GetTerrainSegmentsFor(field, field.maxSegmentLenght);
            var helper = TerrainBuilderHelper.GetNewBuilder();
            if (field.addFilling)
            {
                helper = AddFilling(helper, field.OriginPathSegments, field.fillingLowPoint, field.transform.position);
            } 
            //helper = AddSlopeSegments(helper, terrainSegments);
            //helper = AddFloorSegments(helper, terrainSegments);

            field.mesh.vertices = helper.Vertices;
            field.mesh.triangles = helper.Indices;
            field.mesh.colors = helper.Colors;
            field.mesh.uv = helper.Uvs;

            field.GetComponent<MeshFilter>().mesh = field.mesh;
        }

        private static IEnumerable<TerrainSegments> GetTerrainSegmentsFor(TerrainField field, float maxSegmentLenght)
        {
            var terrainSegments = new List<TerrainSegments>();
            
            var segments = new TerrainSegments();
            var terrainType = TerrainType.Floor;

            foreach (var seg in field.OriginPathSegments)
            {
                var segmentTerrainType = GetTerrainTypeFromSegment(seg);
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

        private static TerrainType GetTerrainTypeFromSegment(LineSegment2D segment)
        {
            if (segment.Slope == null || Math.Abs(segment.Slope.Value) >= 1.0f)
            {
                return TerrainType.Slope;
            }
            return TerrainType.Floor;
        }

        private static ITerrainBuilderHelper AddFloorSegments(ITerrainBuilderHelper helper, IEnumerable<TerrainSegments> terrainSegments)
        {
            var floorSegments = terrainSegments.Where(s => s.TerrainType == TerrainType.Floor);
            foreach(var floorSegment in floorSegments)
            {
                IFloorSegmentBuilder floorHelper = null;
                floorSegment.Segments.ForEach(s => 
                {
                    if(floorHelper == null)
                    {
                        floorHelper = helper.AddFloorSegmentStart(s);
                    }
                    else
                    {
                        floorHelper = floorHelper.AddFloorSegment(s);
                    }
                });
                var lastFloorSegment = floorSegment.Segments.Last();
                helper = floorHelper.AddFloorSegmentEnd(lastFloorSegment.P2, lastFloorSegment.Slope);
            }
            return helper;
        }

        private static ITerrainBuilderHelper AddSlopeSegments(ITerrainBuilderHelper helper, IEnumerable<TerrainSegments> terrainSegments)
        {
            var slopeSegments = terrainSegments.Where(s => s.TerrainType == TerrainType.Slope);
            foreach (var slopeSegment in slopeSegments)
            {
                ISlopeSegmentBuilder slopeHelper = null;
                slopeSegment.Segments.ForEach(s =>
                {
                    if (slopeHelper == null)
                    {
                        slopeHelper = helper.AddSlopeSegmentStart(s);
                    }
                    else
                    {
                        slopeHelper = slopeHelper.AddSlopeSegment(s);
                    }
                });
                var lastSlopeSegment = slopeSegment.Segments.Last();
                helper = slopeHelper.AddSlopeSegmentEnd(lastSlopeSegment.P2, lastSlopeSegment.Slope);
            }
            return helper;
        }

        private static ITerrainBuilderHelper AddFilling(ITerrainBuilderHelper helper, IEnumerable<LineSegment2D> segments, float fillingLowPoint, Vector3 terrainPosition)
        {
            return helper.AddFilling(segments, fillingLowPoint - terrainPosition.y);
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
