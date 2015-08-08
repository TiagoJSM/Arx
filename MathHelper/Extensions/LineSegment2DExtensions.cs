using Extensions;
using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MathHelper.Extensions
{
    public static class LineSegment2DExtensions
    {
        public static int? GetIndexOfPositiveSlope(this LineSegment2D[] segments, int start = 0)
        {
            for (; start < segments.Length; start++)
            {
                if (segments[start].Slope != null && segments[start].Slope.Value > 0)
                {
                    return start;
                }
            }
            return null;
        }

        public static int? GetIndexOfNegativeSlope(this LineSegment2D[] segments, int start = 0)
        {
            for (; start < segments.Length; start++)
            {
                if (segments[start].Slope != null && segments[start].Slope.Value < 0)
                {
                    return start;
                }
            }
            return null;
        }

        public static bool ContainsY(this LineSegment2D segment, float yValue)
        {
            if (segment.P1.y <= yValue && yValue <= segment.P2.y)
            {
                return true;
            }
            if (segment.P2.y <= yValue && yValue <= segment.P1.y)
            {
                return true;
            }
            return false;
        }

        public static IEnumerable<Vector2> GetFillingPolygonVertices(this IEnumerable<LineSegment2D> segments, Tuple<int?, int?> interval, float fillingLowPoint)
        {
            var startIdx = interval.Item1 == null ? 0 : interval.Item1.Value;
            var result = new List<Vector2>();
            if (interval.Item1 == null)
            {
                result.Add(new Vector2(segments.First().P1.x, fillingLowPoint));
                result.Add(segments.First().P1);
            }
            else
            {
                var x = segments.ElementAt(interval.Item1.Value).XWhenYIs(fillingLowPoint);
                result.Add(new Vector2(x, fillingLowPoint));
                startIdx++;
            }

            var endIdx = interval.Item2 == null ? segments.Count() - 1 : interval.Item2.Value;

            while (startIdx < endIdx)
            {
                result.Add(segments.ElementAt(startIdx).P2);
                startIdx++;
            }

            if (interval.Item2 == null)
            {
                result.Add(new Vector2(segments.Last().P2.x, fillingLowPoint));
                result.Add(segments.Last().P2);
            }
            else
            {
                var x = segments.ElementAt(interval.Item2.Value).XWhenYIs(fillingLowPoint);
                result.Add(new Vector2(x, fillingLowPoint));
            }
            return result;
        }
    }
}
