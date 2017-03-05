using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Utility.Editor
{
    public struct MarkeeSelectionRect
    {
        public Vector2 p1;
        public Vector2 p2;

        public Rect Rect
        {
            get
            {
                return new Rect(
                    Mathf.Min(p1.x, p2.x),
                    Mathf.Min(p1.y, p2.y),
                    Math.Abs(p2.x - p1.x),
                    Math.Abs(p2.y - p1.y));
            }
        }
    }
    public static class MarkeeSelection
    {
        public static int[] GetMarkeeSelectionIndices(MarkeeSelectionRect selection, IEnumerable<Vector2> points)
        {
            var rect = selection.Rect;
            return points.IndicesWhere(vec => rect.Contains(vec));
        }
    }
}
