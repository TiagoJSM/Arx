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

        [TestMethod]
        public void TerrainWith2FillingswheresecondIsPartiallyOverTheFirst()
        {
            var segments = new[]
            {
                new LineSegment2D(new Vector2(0.0f, -0.4f), new Vector2(2.2f, 1.4f)),
                new LineSegment2D(new Vector2(2.2f, 1.4f), new Vector2(3.7f, 0.0f)),
                new LineSegment2D(new Vector2(3.7f, 0.0f), new Vector2(3.3f, 2.3f)),
                new LineSegment2D(new Vector2(3.3f, 2.3f), new Vector2(10.4f, 1.6f))
            };

            var intervals = TerrainFillingUtils.GetFillingIntervals(segments, 0);
            Assert.AreEqual(2, intervals.Count());
        }

        [TestMethod]
        public void TerrainWith2ClosedFillings()
        {
            var segments = new[]
            {
                new LineSegment2D(new Vector2(0.0f, -0.4f), new Vector2(2.2f, 1.4f)),
                new LineSegment2D(new Vector2(2.2f, 1.4f), new Vector2(3.9f, -0.3f)),
                new LineSegment2D(new Vector2(3.9f, -0.3f), new Vector2(5.4f, 1.1f)),
                new LineSegment2D(new Vector2(5.4f, 1.1f), new Vector2(9.3f, -1.1f))
            };

            var intervals = TerrainFillingUtils.GetFillingIntervals(segments, 0);
            Assert.AreEqual(2, intervals.Count());
            Assert.AreEqual(new Tuple<int?, int?>(0, 1), intervals.First());
            Assert.AreEqual(new Tuple<int?, int?>(2, 3), intervals.Last());
        }

        [TestMethod]
        public void TerrainWith2OpenFillings()
        {
            var segments = new[]
            {
                new LineSegment2D(new Vector2(0.2f, 0.1f), new Vector2(2.2f, 1.4f)),
                new LineSegment2D(new Vector2(2.2f, 1.4f), new Vector2(3.7f, 0.0f)),
                new LineSegment2D(new Vector2(3.7f, 0.0f), new Vector2(5.3f, 1.5f)),
                new LineSegment2D(new Vector2(5.3f, 1.5f), new Vector2(10.4f, 1.6f))
            };
            var intervals = TerrainFillingUtils.GetFillingIntervals(segments, 0);
            Assert.AreEqual(2, intervals.Count());
            Assert.AreEqual(new Tuple<int?, int?>(null, 1), intervals.First());
            Assert.AreEqual(new Tuple<int?, int?>(2, null), intervals.Last());
        }

        [TestMethod]
        public void TerrainWithLeftOpeningAndFirstSegmentGoingDown()
        {
            var segments = new[]
            {
                new LineSegment2D(new Vector2(0.0f, 0.6f), new Vector2(4.1f, -2.3f)),
                new LineSegment2D(new Vector2(4.1f, -2.3f), new Vector2(7.1f, 1.2f)),
                new LineSegment2D(new Vector2(7.1f, 1.2f), new Vector2(12.7f, -1.5f))
            };
            var intervals = TerrainFillingUtils.GetFillingIntervals(segments, 0);
            Assert.AreEqual(2, intervals.Count());
            Assert.AreEqual(new Tuple<int?, int?>(null, 0), intervals.First());
            Assert.AreEqual(new Tuple<int?, int?>(1, 2), intervals.Last());
        }

        [TestMethod]
        public void TerrainWithLeftOnTopOfFillingLowPointRightOnBellowFillingLowPoint()
        {
            var segments = new[]
            {
                new LineSegment2D(new Vector2(-0.1f, 0.4f), new Vector2(0.8f, -1.1f)),
                new LineSegment2D(new Vector2(0.8f, -1.1f), new Vector2(3.2f, -1.9f))
            };
            var intervals = TerrainFillingUtils.GetFillingIntervals(segments, 0);
            Assert.AreEqual(1, intervals.Count());
            Assert.AreEqual(new Tuple<int?, int?>(null, 0), intervals.First());
        }

        [TestMethod]
        public void TerrainWithLeftOpenAndRightOnOverFillingLowPointAndMidPointOverFillingLowPoint()
        {
            var segments = new[]
            {
                new LineSegment2D(new Vector2(0.0f, -0.3f), new Vector2(2.6f, 0.6f)),
                new LineSegment2D(new Vector2(2.6f, 0.6f), new Vector2(7.6f, 0.7f))
            };
            var intervals = TerrainFillingUtils.GetFillingIntervals(segments, 0);
            Assert.AreEqual(1, intervals.Count());
            Assert.AreEqual(new Tuple<int?, int?>(0, null), intervals.First());
        }

        [TestMethod]
        public void TerrainWithLeftOpenAndRightOnOverFillingLowPointAndMidPointBellowFillingLowPoint()
        {
            var segments = new[]
            {
                new LineSegment2D(new Vector2(0.0f, -0.3f), new Vector2(2.7f, -1.2f)),
                new LineSegment2D(new Vector2(2.7f, -1.2f), new Vector2(7.6f, 0.7f))
            };
            var intervals = TerrainFillingUtils.GetFillingIntervals(segments, 0);
            Assert.AreEqual(1, intervals.Count());
            Assert.AreEqual(new Tuple<int?, int?>(1, null), intervals.First());
        }
    }
}
