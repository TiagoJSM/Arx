using Assets.Standard_Assets.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Hazards.Crusher.Scripts
{
    public class CrusherController : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _crusherSprite;
        [SerializeField]
        private Transform _crusher;
        [SerializeField]
        private Transform _origin;
        [SerializeField]
        private Transform _target;
        [SerializeField]
        private float _crushTime = 0.5f;
        [SerializeField]
        private float _revertTime = 0.5f;
        [SerializeField]
        private float _startDelay = 1.0f;
        [SerializeField]
        private float _vibrationValue = 0.2f;
        [SerializeField]
        private float _vibrationDuration = 0.4f;
        [SerializeField]
        private float _vibrationInterval = 0.05f;

        public IEnumerator Crush()
        {
            while (true)
            {
                yield return new WaitForSeconds(_startDelay);
                yield return CoroutineHelpers.Vibrate(_vibrationDuration, _vibrationValue, _vibrationInterval, _crusherSprite.transform);
                yield return MovementRoutine(_crusher, _origin.position, _target.position, _crushTime);
                yield return MovementRoutine(_crusher, _target.position, _origin.position, _revertTime);
            }
        }

        private IEnumerator MovementRoutine(Transform obj, Vector3 origin, Vector3 target, float duration)
        {
            var elapsed = 0.0f;
            while (elapsed < duration)
            {
                obj.position = MathUtilities.ExponentialInterpolation(origin, target, duration, ref elapsed);
                yield return null;
            }
        }
    }
}
