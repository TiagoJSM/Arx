using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.CharacterBehaviour
{
    public class LowLife : MonoBehaviour
    {
        private Renderer _renderer;
        private AnimationCurve _curve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.25f, 0.5f),
                                                            new Keyframe(0.5f, 0.35f), new Keyframe(0.75f, 0.5f),
                                                            new Keyframe(1f, 0f));

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        void OnWillRenderObject()
        {
            var v = _curve.Evaluate(Time.time % 1.0f);
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(block);
            var color = Color.white;
            color.g = color.g - v;
            color.b = color.b - v;
            block.SetColor("_Color", color);
            _renderer.SetPropertyBlock(block);
        }
    }
}
