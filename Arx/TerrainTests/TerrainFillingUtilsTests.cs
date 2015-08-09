using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathHelper.DataStructures;
using UnityEngine;
using Terrain.Utils;
using System.Linq;
using Extensions;

namespace TerrainTests
{
    [TestClass]
    public class TerrainFillingUtilsTests
    {
        [TestMethod]
        public void TerrainContains2Fillings()
        {
            var segments = new []
            {
                new LineSegment2D(new Vector2(-1.1f, -0.9f), new Vector2(1.0f, 0.9f)),
                new LineSegment2D(new Vector2(1.0f, 0.9f), new Vector2(3.6f, 1.2f)),
                new LineSegment2D(new Vector2(3.6f, 1.2f), new Vector2(7.6f, 0.0f)),
                new LineSegment2D(new Vector2(7.6f, 0.0f), new Vector2(9.8f, -1.8f)),
                new LineSegment2D(new Vector2(9.8f, -1.8f), new Vector2(12.9f, 0.7f))
            };

            var intervals = TerrainFillingUtils.GetFillingIntervals(segments, 0);
            Assert.AreEqual(2, intervals.Count());
            Assert.AreEqual(new Tuple<int?, int?>(0, 2), intervals.First());
            Assert.AreEqual(new Tuple<int?, int?>(4, null), intervals.Last());
        }

        [TestMethod]
        public void TerrainwWith2FillingsAndSlopeInMiddle()
        {
            var segments = new[]
            {
                new LineSegment2D(new Vector2(0.0f, -0.4f), new Vector2(2.2f, 1.4f)),
                new LineSegment2D(new Vector2(2.2f, 1.4f), new Vector2(3.7f, 0.0f)),
                new LineSegment2D(new Vector2(3.7f, 0.0f), new Vector2(4.9f, 1.6f)),
                new LineSegment2D(new Vector2(4.9f, 1.6f), new Vector2(10.4f, 1.6f))
            };

            var intervals = TerrainFillingUtils.GetFillingIntervals(segments, 0);
            Assert.AreEqual(2, intervals.Count());
            Assert.AreEqual(new Tuple<int?, int?>(0, 1), intervals.First());
            Assert.AreEqual(new Tuple<int?, int?>(2, null), intervals.Last());
        }
    }
}
