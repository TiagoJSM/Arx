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
    public class FlyBackToOrigin : MonoBehaviour
    {
        private CharacterController2D _controller;
        private Vector3 _origin;

        [SerializeField]
        private float _speed = 25.0f;

        public Vector3 Origin { get { return _origin; } }

        public IEnumerator GoBackToOrigin()
        {
            return CoroutineHelpers.MoveTo(_controller, () => _origin, _speed);
        }

        private void Awake()
        {
            _controller = GetComponent<CharacterController2D>();
            _origin = transform.position;
        }
    }
}
