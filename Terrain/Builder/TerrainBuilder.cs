using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Terrain.Builder
{
    public class TerrainBuilder
    {
        protected IEnumerable<TerrainSegments> GetTerrainSegmentsFor(IEnumerable<LineSegment2D> lineSegments, TerrainField field)
        {
            var terrainSegments = new List<TerrainSegments>();

            var segments = new TerrainSegments();
            var terrainType = TerrainType.Floor;

            foreach (var seg in lineSegments)
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

        private TerrainType GetTerrainTypeFromSegment(LineSegment2D segment, float floorTerrainMaximumSlope)
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

        private float GetMaxSegmentLenght(TerrainField field, TerrainType segmentTerrainType)
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

        private IEnumerable<LineSegment2D> DivideSegment(LineSegment2D seg, float maxSegmentLenght)
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

    }
}
