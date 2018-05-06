using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Effects.DamageFlash.Scripts
{
    [RequireComponent(typeof(Renderer))]
    public class DefaultColorFlash : MonoBehaviour
    {
        private Renderer _renderer;
        private Coroutine _coroutine;

        [SerializeField]
        private int _flashCount = 5;
        [SerializeField]
        private float _delay = 0.06f;
        [SerializeField]
        private Color _color = Color.white;

        public void Flash()
        {
            if(_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            StartCoroutine(FlashRoutine());
        }

        public IEnumerator FlashRoutine()
        {
            var key = "_FlashAmount";
            for (var idx = 0; idx < _flashCount; idx++)
            {
                _renderer.material.SetFloat(key, 1.0f);
                yield return new WaitForSeconds(_delay / 2);
                _renderer.material.SetFloat(key, 0.0f);
                yield return new WaitForSeconds(_delay / 2);
            }
        }

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _renderer.material.SetColor("_FlashColor", _color);
        }
    }
}
