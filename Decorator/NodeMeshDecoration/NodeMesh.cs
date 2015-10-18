using GenericComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Decorator.NodeMeshDecoration
{
    public class NodeMesh : NodePath
    {
        public Mesh mesh;
        [Header("Mesh shape")]
        public float meshWidth = 0.5f;

    }
}
