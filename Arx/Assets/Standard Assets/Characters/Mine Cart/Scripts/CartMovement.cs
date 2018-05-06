using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Mine_Cart.Scripts
{
    [RequireComponent(typeof(CharacterController2D))]
    public class CartMovement : MonoBehaviour
    {
        private CharacterController2D _characterController;
        private float _jumpTime;
        private Vector2 _velocity;

        [SerializeField]
        private float _gravity = -30;
        [SerializeField]
        private float _horizontalVelocity = 4;
        [SerializeField]
        private float _jump = 50;
        [SerializeField]
        private float _maxJumpTime = 0.6f;
        [SerializeField]
        private float _minJumpTime = 0.1f;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController2D>();
        }

        private void Update()
        {
            var inputDevice = InputManager.Instance.GetInputDevice();
            var gravity = _gravity;
            var horizontal = 0.0f;
            if (inputDevice.GetButton(DeviceButton.Jump) && _jumpTime < _maxJumpTime || (_jumpTime > 0.0 && _jumpTime < _minJumpTime))
            {
                _jumpTime += Time.deltaTime;
                _velocity.y += _jump * Time.deltaTime;
                gravity = 0.0f;
            }
            var axis = inputDevice.GetAxis(DeviceAxis.Movement);
            if (!Mathf.Approximately(axis.x, 0.0f))
            {
                horizontal = axis.x > 0 ? _horizontalVelocity : -_horizontalVelocity;
            }
            if (inputDevice.GetButtonUp(DeviceButton.Jump))
            {
                _jumpTime = float.MaxValue;
            }
            if (_characterController.isGrounded)
            {
                _jumpTime = 0.0f;
            }

            //_velocity.x = _horizontalVelocity;
            _velocity.x = horizontal;
            _velocity.y += gravity * Time.deltaTime;
            _characterController.move(_velocity * Time.deltaTime);
            _velocity = _characterController.velocity;
        }
    }
}
