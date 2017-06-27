using Assets.Standard_Assets.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Platforms.Gate.Scripts
{
    public class GateBehaviour : MonoBehaviour
    {
        private Coroutine _gateRoutine;

        [SerializeField]
        private Transform _gate;
        [SerializeField]
        private float _openTime = 1;
        [SerializeField]
        private float _closeTime = 1;
        [SerializeField]
        private float _waitTime = 2;
        [SerializeField]
        private Transform _openPosition;

        public void Open()
        {
            if(_gateRoutine == null)
            {
                _gateRoutine = StartCoroutine(GateRoutine());
            }
        }

        private IEnumerator GateRoutine()
        {
            var origin = transform.position;
            yield return CoroutineHelpers.MoveTo(origin, _openPosition.position, _openTime, _gate);
            yield return new WaitForSeconds(_waitTime);
            yield return CoroutineHelpers.MoveTo(_openPosition.position, origin, _closeTime, _gate);
            _gateRoutine = null;
        }
    }
}
