using Extensions;
using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathHelper.Extensions;

namespace Terrain.Utils
{
    public static class TerrainFillingUtils
    {
        public static IEnumerable<Tuple<int?, int?>> GetFillingIntervals(LineSegment2D[] segments, float fillingLowPoint)
        {
            var startSegmentIndex = GetIndexOfIntersectedSlope(segments, fillingLowPoint);
            var endSegmentIndex = GetIndexOfIntersectedSlope(segments, fillingLowPoint, 1);

            /*if (startSegmentIndex == null && endSegmentIndex == null)
            {
                return new Tuple<int?, int?>[0];
            }*/
            if (startSegmentIndex == null || endSegmentIndex == null)
            {
                return new[] { new Tuple<int?, int?>(startSegmentIndex, endSegmentIndex) };
            }

            var result = new List<Tuple<int?, int?>>();

            if (startSegmentIndex.Value >= endSegmentIndex.Value)
            {
                result.Add(new Tuple<int?, int?>(null, endSegmentIndex));
            }
            else
            {
                result.Add(new Tuple<int?, int?>(startSegmentIndex, endSegmentIndex));
            }

            var idx = endSegmentIndex.Value + 1;
            while (idx < segments.Count())
            {
                if (segments[idx].P2.y < fillingLowPoint)
                {
                    idx++;
                    continue;
                }
                startSegmentIndex = GetIndexOfIntersectedSlope(segments, fillingLowPoint, idx);
                if (startSegmentIndex == null)
                {
                    break;
                }
                idx = startSegmentIndex.Value + 1;
                endSegmentIndex = GetIndexOfIntersectedSlope(segments, fillingLowPoint, idx);
                result.Add(new Tuple<int?, int?>(startSegmentIndex, endSegmentIndex));
                if (endSegmentIndex == null)
                {
                    break;
                }
                idx++;
            }

            return result;
        }

        private static int? GetIndexOfIntersectedSlope(LineSegment2D[] segments, float fillingLowPoint, int start = 0)
        {
            while (start < segments.Count())
            {
                var segment = segments[start];
                if (segment.ContainsY(fillingLowPoint))
                {
                    return start;
                }
                start++;
            }
            return null;
        }

        /*private static int? GetIndexOfPositiveIntersectedSlope(LineSegment2D[] segments, float fillingLowPoint, int start = 0)
        {
            while (start < segments.Count())
            {
                var segmentWithPositiveSlopeIndex = segments.GetIndexOfPositiveSlope(start);
                if (segmentWithPositiveSlopeIndex != null)
                {
                    if (segments[segmentWithPositiveSlopeIndex.Value].ContainsY(fillingLowPoint))
                    {
                        return segmentWithPositiveSlopeIndex;
                    }
                }
                start++;
            }
            return null;
        }

        private static int? GetIndexOfNegativeIntersectedSlope(LineSegment2D[] segments, float fillingLowPoint, int start = 0)
        {
            while (start < segments.Count())
            {
                var segmentWithNegativeSlopeIndex = segments.GetIndexOfNegativeSlope(start);
                if (segmentWithNegativeSlopeIndex != null)
                {
                    if (segments[segmentWithNegativeSlopeIndex.Value].ContainsY(fillingLowPoint))
                    {
                        return segmentWithNegativeSlopeIndex;
                    }
                }
                start++;
            }
            return null;
        }*/
    }
}
