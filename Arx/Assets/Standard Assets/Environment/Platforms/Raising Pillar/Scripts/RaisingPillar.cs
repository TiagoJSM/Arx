using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Platforms.Raising_Pillar.Scripts
{
    public delegate void OnRiseComplete(RaisingPillar pillar);
    [RequireComponent(typeof(Animator))]
    public class RaisingPillar : MonoBehaviour
    {
        private const string Shake = "Shake";
        private readonly int ShakeId = Animator.StringToHash(Shake);

        private Animator _animator;
        private Coroutine _raiseRoutine;
        private Vector3 _startPosition;
        private float _elapsed;

        [SerializeField]
        private float _moveToTargetTime = 2;
        [SerializeField]
        private float _moveToOriginTime = 2;
        [SerializeField]
        private float _activationTime = 2;
        [SerializeField]
        private float _holdTime = 2;
        [SerializeField]
        private Transform _target;

        public event OnRiseComplete OnRiseComplete;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _startPosition = transform.position;
        }

        public void Raise()
        {
            if(_raiseRoutine == null)
            {
                _raiseRoutine = StartCoroutine(RaiseFunc());
            }
        }

        private IEnumerator RaiseFunc()
        {
            _animator.SetBool(ShakeId, true);
            yield return new WaitForSeconds(_activationTime);
            _animator.SetBool(ShakeId, false);
            yield return MovePillar(_target.position, _moveToTargetTime);
            yield return new WaitForSeconds(_holdTime);
            yield return MovePillar(_startPosition, _moveToOriginTime);
            _raiseRoutine = null;

            if(OnRiseComplete != null)
            {
                OnRiseComplete(this);
            }
        }

        private IEnumerator MovePillar(Vector3 target, float time)
        {
            _elapsed = 0;
            var startPosition = transform.position;
            while (_elapsed <= time)
            {
                transform.position = Vector3.Lerp(startPosition, target, _elapsed / time);
                _elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}
