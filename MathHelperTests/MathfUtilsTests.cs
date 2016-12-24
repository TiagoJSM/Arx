using MathHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MathHelperTests
{
    [TestClass]
    public class MathfUtilsTests
    {
        [TestMethod]
        public void ParabolaValuesAreCorrectlyFound()
        {
            var p0 = new Vector2(-1, 0);
            var p1 = new Vector2(2, 0);
            var p2 = new Vector2(0, -2);
            var parabola = MathfUtils.GetParabola(p0, p1, p2);
            Assert.AreEqual(1, parabola.a, 0.1f);
            Assert.AreEqual(-1, parabola.b, 0.1f);
            Assert.AreEqual(-2, parabola.c, 0.1f);
        }
    }
}
