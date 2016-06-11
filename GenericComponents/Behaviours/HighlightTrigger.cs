using CommonInterfaces.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Behaviours
{
    public class HighlightTrigger : MonoBehaviour
    {
        private const string ColorName = "_Color";
        private const string ColorMultiplierName = "_ColorMultiplier";

        private Material _materialToAssign;
        private Color _tint;
        private float _colorMultiplier;
        private float _elapsedTime;
        private Coroutine _pulseRoutine;

        public AnimationCurve pulse;
        public GameObject interactable;
        public Material material;

        void Start()
        {
            _materialToAssign = new Material(material);
            _tint = _materialToAssign.GetColor(ColorName);
            _colorMultiplier = _materialToAssign.GetFloat(ColorMultiplierName);
            _materialToAssign.SetFloat(ColorMultiplierName, 1);
            _materialToAssign.SetColor(ColorName, Color.white);

            var renderers = interactable.GetComponentsInChildren<Renderer>();
            foreach (var spriteRenderer in renderers)
            {
                spriteRenderer.material = _materialToAssign;
            }
            renderers = interactable.GetComponents<Renderer>();
            foreach (var spriteRenderer in renderers)
            {
                spriteRenderer.material = _materialToAssign;
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            var playerControl = collider.gameObject.GetComponent<IPlayerControl>();
            if (playerControl == null)
            {
                return;
            }
            StartPulse();
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            var playerControl = collider.gameObject.GetComponent<IPlayerControl>();
            if (playerControl == null)
            {
                return;
            }
            StopPulse();
        }

        private void StartPulse()
        {
            if(_pulseRoutine != null)
            {
                return;
            }
            
            _elapsedTime = 0;
            _materialToAssign.SetFloat(ColorMultiplierName, _colorMultiplier);
            _materialToAssign.SetColor(ColorName, _tint);
            _pulseRoutine = StartCoroutine(Pulse());
        }

        private void StopPulse()
        {
            if(_pulseRoutine != null)
            {
                
                _materialToAssign.SetColor(ColorName, Color.white);
                _materialToAssign.SetFloat(ColorMultiplierName, 1);
                StopCoroutine(_pulseRoutine);
                _pulseRoutine = null;
            }
        }

        //void Update()
        //{
        //    _elapsedTime += Time.deltaTime;
        //    var value = pulse.Evaluate(_elapsedTime);
        //    _materialToAssign.SetFloat(ColorMultiplierName, _colorMultiplier + value);
        //}

        private IEnumerator Pulse()
        {
            while (true)
            {
                _elapsedTime += Time.deltaTime;
                var value = pulse.Evaluate(_elapsedTime);
                _materialToAssign.SetFloat(ColorMultiplierName, _colorMultiplier + value);
                yield return null;
            }
        }
    }
}
