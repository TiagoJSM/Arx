using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Decorator.Effects
{
    public class CameraBoundTextureOffsetMesh : MonoBehaviour
    {
        private Renderer _renderer;
        private Vector3? _previousPosition;

        public Vector2 offsetDirection = new Vector2(1, 0);
        public float scrollSpeed = 0.5f;
        public float cameraFactor = 1.0f;

        void Start()
        {
            _renderer = this.gameObject.GetComponent<Renderer>();
        }

        void Update()
        {
            var camera = Camera.main;
            if (camera == null)
            {
                return;
            }
            if (_previousPosition == null)
            {
                _previousPosition = camera.transform.position;
                //return;
            }

            var cameraPosition = camera.transform.position;
            var positionDif = (cameraPosition - _previousPosition.Value).ToVector2();
            var offset = (Time.time * scrollSpeed * offsetDirection) + (positionDif * cameraFactor);
            _renderer.material.mainTextureOffset = offset;
        }
    }
}
