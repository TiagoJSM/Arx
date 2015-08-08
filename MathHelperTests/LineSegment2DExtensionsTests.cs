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
    }
}
