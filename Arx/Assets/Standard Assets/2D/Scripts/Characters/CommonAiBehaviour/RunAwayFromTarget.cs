using Assets.Standard_Assets.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Characters.CommonAiBehaviour
{
    [RequireComponent(typeof(CharacterController2D))]
    public class RunAwayFromTarget : MonoBehaviour
    {
        private CharacterController2D _controller;

        [SerializeField]
        private float _speed = 25.0f;
        [SerializeField]
        private float _distance = 6.0f;

        public IEnumerator RunAway(GameObject target, Action onComplete)
        {
            var oppositeDirection = (transform.position - target.transform.position).normalized;
            var targetPosition = transform.position + _distance * oppositeDirection;
            return CoroutineHelpers.MoveTo(_controller, () => targetPosition, _speed, onComplete: onComplete);
        }

        private void Awake()
        {
            _controller = GetComponent<CharacterController2D>();
        }
    }
}
