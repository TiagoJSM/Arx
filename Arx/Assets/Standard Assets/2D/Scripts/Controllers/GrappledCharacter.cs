using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Controllers
{
    [RequireComponent(typeof(CharacterController2D))]
    public class GrappledCharacter : MonoBehaviour
    {
        private CharacterController2D _characterController;

        private GameObject _grapple;
        private GameObject _grappler;
        private Vector3 _grappleOffset;
        private Coroutine _pullRoutine;

        public bool Grappled { get { return _grapple != null; } }
        public GameObject Grappler { get { return _grappler; } }

        public event Action OnGrappled;

        public void StartGrapple(GameObject grapple, GameObject grappler)
        {
            _grapple = grapple;
            _grappler = grappler;
            _grappleOffset = _characterController.transform.position - grapple.transform.position;
            if(OnGrappled != null)
            {
                OnGrappled();
            }
        }

        public void Pull()
        {
            _pullRoutine = StartCoroutine(PullRoutine());
        }

        public void EndGrapple()
        {
            _grapple = null;
            if (_pullRoutine != null)
            {
                StopCoroutine(_pullRoutine);
                _pullRoutine = null;
            }
        }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController2D>();
        }

        private IEnumerator PullRoutine()
        {
            while (true)
            {
                if(_grapple == null)
                {
                    _pullRoutine = null;
                    yield break;
                }
                var move = _grapple.transform.position - _characterController.transform.position + _grappleOffset;
                _characterController.move(move);
                yield return null;
            }
        }
    }
}
