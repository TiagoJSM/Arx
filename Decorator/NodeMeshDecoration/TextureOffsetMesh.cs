using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Decorator.NodeMeshDecoration
{
    [RequireComponent(typeof(NodeMesh))]
    public class TextureOffsetMesh : MonoBehaviour
    {
        private NodeMesh _nodeMesh;

        public Vector2 offsetDirection = new Vector2(1, 0);
        public float scrollSpeed = 0.5f;

        void Start()
        {
            _nodeMesh = GetComponent<NodeMesh>();
        }

        void Update()
        {
            var renderer = _nodeMesh.GetComponent<Renderer>();
            var offset = Time.time * scrollSpeed * offsetDirection;
            renderer.material.SetTextureOffset("_MainTex", offset);
        }
    }
}
