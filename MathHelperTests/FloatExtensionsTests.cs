using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathHelper.Extensions;

namespace MathHelperTests
{
    [TestClass]
    public class FloatExtensionsTests
    {
        [TestMethod]
        public void ReduceNegativeRadianToQuadrant0Or3()
        {
            var radians = -0.6599129f;
            radians = radians.ReduceToQuadrant0and3();
            Assert.AreEqual(5.62327242f, radians, 0.00001f);
        }

        [TestMethod]
        public void IsInNegativeXQuadrant()
        {
            Assert.IsTrue((1.96108f).IsInNegativeXQuadrant());
        }

        [TestMethod]
        public void IsInPositiveXQuadrant()
        {
            Assert.IsTrue((0.2153008f).IsInPositiveXQuadrant());
        }

        [TestMethod]
        public void NormalizeNegativeRadianValue()
        {
            var radians = -0.2336121f;
            radians = radians.NormalizeRadians();
        }
    }
}
