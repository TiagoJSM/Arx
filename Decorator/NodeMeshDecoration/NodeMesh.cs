using GenericComponents;
using GenericComponents.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Decorator.NodeMeshDecoration
{
    public class NodeMesh : NodePathBehaviour
    {
        public Mesh mesh;
        [Header("Mesh shape")]
        public float meshWidth = 0.5f;
        [Header("Texturing")]
        public Shader shader;

    }
}
