using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Platforms.PressurePlatform.Scripts
{
    public class PressurePlatform : MonoBehaviour
    {
        private Vector3 _startingPoint;

        [SerializeField]
        private Transform _target;
        [SerializeField]
        private float _delay;
        [SerializeField]
        private float _holdTime = 1;
        [SerializeField]
        private float _restTime = 1;
        [SerializeField]
        private float _timeToReachTarget = 1;
        [SerializeField]
        private float _timeToReturn = 1;

        private void Start()
        {
            _startingPoint = transform.position;
            StartCoroutine(MovementCoroutine());
        }

        private IEnumerator MovementCoroutine()
        {
            yield return new WaitForSeconds(_delay);

            while (true)
            {
                yield return MoveTo(_target.position, _timeToReachTarget);
                yield return new WaitForSeconds(_holdTime);
                yield return MoveTo(_startingPoint, _timeToReturn);
                yield return new WaitForSeconds(_restTime);
            }
        }

        private IEnumerator MoveTo(Vector3 target, float time)
        {
            var startTime = Time.time;

            var start = transform.position;
            while (true)
            {
                var elapsed = Time.time - startTime;
                var ratio = elapsed / time;
                if (ratio > 1)
                {
                    yield break;
                }
                transform.position = Vector3.Lerp(start, target, ratio);
                yield return null;
            }
        }
    }
}
