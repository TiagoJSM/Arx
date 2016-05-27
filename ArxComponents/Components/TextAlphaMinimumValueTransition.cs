using CommonInterfaces.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ArxGame.Components
{
    public class TextAlphaMinimumValueTransition : MonoBehaviour
    {
        private const string HidePropertyName = "_MinimumValue";
        private Material _material;
        private float _hideValue = 1;
        private IEnumerator _hideCoroutine;

        public float VariationPerSecond = 1.0f;
        public Text text;

        void Start()
        {
            
            _material = text.materialForRendering;
            _material.SetFloat(HidePropertyName, _hideValue);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            _material = text.material;
            if (other.GetComponent<IPlayerControl>() == null)
            {
                return;
            }
            StopCoroutine();
            _hideCoroutine = ShowText();
            StartCoroutine(_hideCoroutine);
        }

        void OnTriggerExit2D(Collider2D other)
        {
            _material = text.material;
            if (other.GetComponent<IPlayerControl>() == null)
            {
                return;
            }
            StopCoroutine();
            _hideCoroutine = HideText();
            StartCoroutine(_hideCoroutine);
        }

        private void StopCoroutine()
        {
            if (_hideCoroutine != null)
            {
                StopCoroutine(_hideCoroutine);
            }
        }

        private IEnumerator HideText()
        {
            while (true)
            {
                var elapsedValue = Time.deltaTime * VariationPerSecond;
                _hideValue = _hideValue + elapsedValue;

                if (_hideValue > 1.0f)
                {
                    _hideValue = 1.0f;
                    _material.SetFloat(HidePropertyName, _hideValue);
                    yield break;
                }

                _material.SetFloat(HidePropertyName, _hideValue);
                yield return null;
            }
        }

        private IEnumerator ShowText()
        {
            while (true)
            {
                var elapsedValue = Time.deltaTime * VariationPerSecond;
                _hideValue = _hideValue - elapsedValue;

                if (_hideValue < 0.0f)
                {
                    _hideValue = 0.0f;
                    _material.SetFloat(HidePropertyName, _hideValue);
                    yield break;
                }

                _material.SetFloat(HidePropertyName, _hideValue);
                yield return null;
            }
        }
    }
}
