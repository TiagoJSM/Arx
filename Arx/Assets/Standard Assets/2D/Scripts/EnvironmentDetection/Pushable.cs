using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.EnvironmentDetection
{
    [RequireComponent(typeof(CharacterController2D))]
    public class Pushable : MonoBehaviour
    {
        private Vector3 _velocity;
        private float? _force;

        private CharacterController2D _characterController2D;

        [SerializeField]
        private float _gravity = -25f;
        [SerializeField]
        private BoxCollider2D _collider;

        public void Push(float force)
        {
            _force = force;
        }

        private void Start()
        {
            _characterController2D = GetComponent<CharacterController2D>();
            _characterController2D.BoxCollider2D = _collider;
        }

        private void Update()
        {
            _velocity.x = 0;
            _velocity.y += _gravity * Time.deltaTime;
            if (_force != null)
            {
                _velocity.x = _force.Value;
            }
            _force = null;
            _characterController2D.move(_velocity * Time.deltaTime);
            _velocity = _characterController2D.velocity;
        }
    }
}
