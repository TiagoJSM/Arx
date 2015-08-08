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
            var segmentWithPositiveSlopeIndex = segments.GetIndexOfPositiveSlope();
            var segmentWithNegativeSlopeIndex = segments.GetIndexOfNegativeSlope();

            if (segmentWithPositiveSlopeIndex == null && segmentWithNegativeSlopeIndex == null)
            {
                return new Tuple<int?, int?>[0];
            }
            if (segmentWithPositiveSlopeIndex == null || segmentWithNegativeSlopeIndex == null)
            {
                return new[] { new Tuple<int?, int?>(segmentWithPositiveSlopeIndex, segmentWithNegativeSlopeIndex) };
            }

            var result = new List<Tuple<int?, int?>>();

            if (segmentWithPositiveSlopeIndex.Value > segmentWithNegativeSlopeIndex.Value)
            {
                result.Add(new Tuple<int?, int?>(null, segmentWithNegativeSlopeIndex));
            }
            else
            {
                result.Add(new Tuple<int?, int?>(segmentWithPositiveSlopeIndex, segmentWithNegativeSlopeIndex));
            }

            var idx = segmentWithNegativeSlopeIndex.Value;
            while (idx < segments.Count())
            {
                segmentWithPositiveSlopeIndex = GetIndexOfPositiveIntersectedSlope(segments, fillingLowPoint, idx);
                if (segmentWithPositiveSlopeIndex == null)
                {
                    break;
                }
                idx = segmentWithPositiveSlopeIndex.Value;
                segmentWithNegativeSlopeIndex = GetIndexOfNegativeIntersectedSlope(segments, fillingLowPoint, idx);
                result.Add(new Tuple<int?, int?>(segmentWithPositiveSlopeIndex, segmentWithNegativeSlopeIndex));
                if (segmentWithNegativeSlopeIndex == null)
                {
                    break;
                }
            }

            return result;
        }

        private static int? GetIndexOfPositiveIntersectedSlope(LineSegment2D[] segments, float fillingLowPoint, int start = 0)
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
        }
    }
}
