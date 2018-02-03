using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Fire.Scripts
{
    [RequireComponent(typeof(Light))]
    public class FireLightFlicker : MonoBehaviour
    {
        private Light _light;
        private float _originalRange;
        private float _originalIntensity;
        private Vector3 _originalPosition;

        [SerializeField]
        private float _rangeVariation = 1.0f;
        [SerializeField]
        private float _minimumVariationInterval = 0.4f;
        [SerializeField]
        private float _maximumVariationInterval = 1.0f;
        [SerializeField]
        private float _intensityVariation = 1.0f;
        [SerializeField]
        private float _positionVariation = 0.2f;

        private void Awake()
        {
            _light = GetComponent<Light>();
            _originalRange = _light.range;
            _originalIntensity = _light.intensity;
            _originalPosition = _light.transform.position;

            StartCoroutine(LightFlickerRoutine());
        }

        private IEnumerator LightFlickerRoutine()
        {
            while (true)
            {
                var rangeVariation = UnityEngine.Random.Range(-_rangeVariation, _rangeVariation);
                var intensityVariation = UnityEngine.Random.Range(-_intensityVariation, _intensityVariation);
                Vector3 positionVariation = UnityEngine.Random.insideUnitCircle * _positionVariation;
                var interval = UnityEngine.Random.Range(_minimumVariationInterval, _maximumVariationInterval);

                var currentRange = _light.range;
                var targetRange = _originalRange + rangeVariation;

                var currentIntensity = _light.intensity;
                var targetIntensity = _originalIntensity + intensityVariation;

                var currentPosition = _light.transform.position;
                var targetPosition = _originalPosition + positionVariation;

               var elapsed = 0.0f;
                while(elapsed < interval)
                {
                    var elapsedUnit = elapsed / interval;
                    _light.range = Mathf.Lerp(currentRange, targetRange, elapsedUnit);
                    _light.intensity = Mathf.Lerp(currentIntensity, targetIntensity, elapsedUnit);
                    _light.transform.position = Vector3.Lerp(currentPosition, targetPosition, elapsedUnit);
                    elapsed += Time.deltaTime;
                    yield return null;
                }
            }
        }
    }
}
