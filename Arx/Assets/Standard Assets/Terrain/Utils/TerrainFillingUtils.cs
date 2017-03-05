using Extensions;
using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathHelper.Extensions;

namespace Assets.Standard_Assets.Terrain.Utils
{
    public static class TerrainFillingUtils
    {
        public static IEnumerable<Tuple<int?, int?>> GetFillingIntervals(LineSegment2D[] segments, float fillingLowPoint)
        {
            var intersectionIndexes = GetIndexOfIntersectedSegments(segments, fillingLowPoint);

            if (segments.First().P1.y > fillingLowPoint)
            {
                intersectionIndexes.Insert(0, null);
            }

            if (segments.Last().P2.y > fillingLowPoint)
            {
                intersectionIndexes.Add(null);
            }

            var start = default(int?);
            var hasStart = false;
            for (var idx = 0; idx < intersectionIndexes.Count(); idx++)
            {
                var segmentIndex = default(int?);
                if (!hasStart)
                {
                    segmentIndex = intersectionIndexes[idx];
                    if (IsFillingStartIndex(segments, segmentIndex))
                    {
                        start = segmentIndex;
                        hasStart = true;
                    }
                    continue;
                }
                
                segmentIndex = intersectionIndexes[idx];
                if (!IsFillingEndIndex(segments, segmentIndex))
                {
                    continue;
                }

                hasStart = false;
                yield return new Tuple<int?, int?>(start, segmentIndex);
            }
        }

        private static bool IsFillingStartIndex(LineSegment2D[] segments, int? index)
        {
            if (index == null)
            {
                return true;
            }
            return segments[index.Value].PositiveSlope;
        }

        private static bool IsFillingEndIndex(LineSegment2D[] segments, int? index)
        {
            if (index == null)
            {
                return true;
            }
            return segments[index.Value].NegativeSlope;
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

        private static List<int?> GetIndexOfIntersectedSegments(LineSegment2D[] segments, float fillingLowPoint)
        {
            var result = new List<int?>();
            for (var idx = 0; idx < segments.Length; idx++)
            {
                var segment = segments[idx];
                if (segment.ContainsY(fillingLowPoint))
                {
                    result.Add(idx);
                }
            }
            return result;
        }
    }
}
