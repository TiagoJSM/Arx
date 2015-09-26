using MathHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathHelperTests
{
    [TestClass]
    public class FloatUtilsTests
    {
        [TestMethod]
        public void AIsBiggerThanBButInsideTolerance()
        {
            var a = 5;
            var b = 4;
            var tolerance = 2;
            Assert.AreEqual(true, FloatUtils.IsApproximately(a, b, tolerance));
        }

        [TestMethod]
        public void AIsBiggerThanBButOutsideTolerance()
        {
            var a = 5;
            var b = 4;
            var tolerance = 0.5f;
            Assert.AreEqual(false, FloatUtils.IsApproximately(a, b, tolerance));
        }
    }
}
