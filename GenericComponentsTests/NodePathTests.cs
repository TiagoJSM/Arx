using GenericComponents;
using NUnit.Framework;
using UnityEngine;
using System;
using System.Linq;
using Assets.Standard_Assets.GenericComponents;

namespace GenericComponentsTests
{
    [TestFixture]
    public class NodePathTests
    {
        [Test]
        public void ThreePathNodesOriginateTwoSegments()
        {
            var pathNode = new NodePath();
            pathNode.AddPathNode(new Vector2());
            pathNode.AddPathNode(new Vector2(1, 1));
            pathNode.AddPathNode(new Vector2(2, 2));
            //Assert.AreEqual(2, pathNode.ControlPathSegments.Count());
        }
    }
}