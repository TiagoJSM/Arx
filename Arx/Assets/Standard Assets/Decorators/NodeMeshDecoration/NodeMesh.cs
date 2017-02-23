using Assets.Standard_Assets.GenericComponents.Behaviours;
using GenericComponents;
using GenericComponents.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Decorators.NodeMeshDecoration
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
