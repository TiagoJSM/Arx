using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Scripts.RendererEffects
{
    [RequireComponent(typeof(Renderer))]
    public class TextureOffsetOverTime : MonoBehaviour
    {
        private Renderer _renderer;

        public Vector2 scrollSpeed = Vector2.one;
        
        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            var offset = Time.time * scrollSpeed;
            _renderer.material.mainTextureOffset = offset;
        }
    }
}
