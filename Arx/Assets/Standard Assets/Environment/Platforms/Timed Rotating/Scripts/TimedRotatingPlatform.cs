using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Platforms.Timed_Rotating.Scripts
{
    public class TimedRotatingPlatform : MonoBehaviour
    {
        [SerializeField]
        private int _totalRotations = 2;
        [SerializeField]
        private float _shakingTime = 1.0f;
        [SerializeField]
        private float _rotatingTime = 0.5f;
        [SerializeField]
        private float _stillTime = 4.0f;
        [SerializeField]
        private bool _clockwork = true;
        [SerializeField]
        private Animator _animator;

        private void Start()
        {
            _animator.enabled = false;
            StartCoroutine(RotatingRoutine());
        }

        private IEnumerator RotatingRoutine()
        {
            var originalRotation = _animator.transform.eulerAngles.z;
            var totalRotations = _totalRotations > 0 ? _totalRotations : 2;
            var rotationPerCycle = (360.0f / totalRotations) * (_clockwork ? -1 : 1);
            while (true)
            {
                yield return new WaitForSeconds(_stillTime);
                _animator.enabled = true;
                yield return new WaitForSeconds(_shakingTime);
                _animator.enabled = false;
                _animator.transform.rotation = Quaternion.Euler(0, 0, originalRotation);
                var totalTime = 0.0f;
                var startRotation = transform.rotation.eulerAngles.z;

                while(_rotatingTime >= totalTime)
                {
                    var rotation = Mathf.Lerp(0, rotationPerCycle, totalTime / _rotatingTime);
                    transform.rotation = Quaternion.Euler(0, 0, startRotation + rotation);
                    totalTime += Time.deltaTime;
                    yield return null;
                }
                transform.rotation = Quaternion.Euler(0, 0, startRotation + rotationPerCycle);
            }
        }
    }
}
