using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using MathHelper.DataStructures;
using UnityEngine;
using MathHelper.Extensions;
using Extensions;
using System.Linq;

namespace MathHelperTests
{
    [TestClass]
    public class LineSegment2DExtensionsTests
    {
        [TestMethod]
        public void PolygonWithoutStartIndex()
        {
            var segments = new List<LineSegment2D>()
            {
                new LineSegment2D(new Vector2(0, 1), new Vector2(1, 2)),
                new LineSegment2D(new Vector2(1, 2), new Vector2(-1, 4))
            };

            var vectors = segments.GetFillingPolygonVertices(new Tuple<int?, int?>(null, 1), 0);
            Assert.AreEqual(4, vectors.Count());
            Assert.AreEqual(new Vector2(), vectors.ElementAt(0));
            Assert.AreEqual(new Vector2(0, 1), vectors.ElementAt(1));
            Assert.AreEqual(new Vector2(1, 2), vectors.ElementAt(2));
            Assert.AreEqual(new Vector2(3, 0), vectors.ElementAt(3));
        }

        [TestMethod]
        public void TwoPolygonsLastWithoutEnding()
        {
            var segments = new List<LineSegment2D>()
            {
                new LineSegment2D(new Vector2(0, -0.4f), new Vector2(2.2f, 1.4f)),
                new LineSegment2D(new Vector2(2.2f, 1.4f), new Vector2(4.5f, 0.3f)),
                new LineSegment2D(new Vector2(4.5f, 0.3f), new Vector2(7.0f, -1.2f)),
                new LineSegment2D(new Vector2(7.0f, -1.2f), new Vector2(10.4f, 1.6f))
            };

            var polygon1Vertices = segments.GetFillingPolygonVertices(new Tuple<int?, int?>(0, 2), 0);
            Assert.AreEqual(4, polygon1Vertices.Count());
            var polygon2Vertices = segments.GetFillingPolygonVertices(new Tuple<int?, int?>(3, null), 0);
            Assert.AreEqual(3, polygon2Vertices.Count());
        }

        [TestMethod]
        public void TwoPolygonsDivideByVertexInLowPoint()
        {
            var segments = new[]
            {
                new LineSegment2D(new Vector2(0.0f, -0.4f), new Vector2(2.2f, 1.4f)),
                new LineSegment2D(new Vector2(2.2f, 1.4f), new Vector2(3.7f, 0.0f)),
                new LineSegment2D(new Vector2(3.7f, 0.0f), new Vector2(4.9f, 1.6f)),
                new LineSegment2D(new Vector2(4.9f, 1.6f), new Vector2(10.4f, 1.6f))
            };

            var polygon1Vertices = segments.GetFillingPolygonVertices(new Tuple<int?, int?>(0, 1), 0);
            Assert.AreEqual(3, polygon1Vertices.Count());
            var polygon2Vertices = segments.GetFillingPolygonVertices(new Tuple<int?, int?>(2, null), 0);
            Assert.AreEqual(4, polygon2Vertices.Count());
        }
    }
}
