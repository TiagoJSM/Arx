using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Decorator.Effects
{
    public class TextureOffsetMesh : MonoBehaviour
    {
        private Renderer _renderer;

        public Vector2 offsetDirection = new Vector2(1, 0);
        public float scrollSpeed = 0.5f;

        void Start()
        {
            _renderer = this.gameObject.GetComponent<Renderer>();
        }

        void Update()
        {
            var offset = Time.time * scrollSpeed * offsetDirection;
            _renderer.material.mainTextureOffset = offset;
        }
    }
}
