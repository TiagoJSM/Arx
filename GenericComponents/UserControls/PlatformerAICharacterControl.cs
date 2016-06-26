using GenericComponents.Controllers.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using Utils;
using CommonInterfaces.Controls;
using System.Collections;
using GenericComponents.Controllers.AnimationControllers;

namespace GenericComponents.UserControls
{
    [RequireComponent(typeof(PlatformerCharacterController))]
    [RequireComponent(typeof(PlatformerCharacterAnimationController))]
    public abstract class PlatformerAICharacterControl : MonoBehaviour, IPlatformerAICharacterControl
    {
        private PlatformerCharacterAnimationController _animationController;
        private PlatformerCharacterController _characterController;
        private Vector2? _moveToPosition;
        private float _treshold;

        protected PlatformerCharacterController CharacterController
        {
            get
            {
                return _characterController;
            }
        }
        protected PlatformerCharacterAnimationController AnimationController
        {
            get
            {
                return _animationController;
            }
        }

        public void MoveDirectlyTo(Vector2 position, float treshold)
        {
            _moveToPosition = position;
            _treshold = treshold;
        }

        public void StopMoving()
        {
            _moveToPosition = null;
        }

        protected void PerformStart()
        {
            _characterController = GetComponent<PlatformerCharacterController>();
            _animationController = GetComponent<PlatformerCharacterAnimationController>();
        }

        protected void PerformFixedUpdate()
        {
            if (_moveToPosition == null)
            {
                return;
            }

            var currentPosition = this.transform.position.ToVector2();
            var distance = Vector2.Distance(currentPosition, _moveToPosition.Value);
            if (distance < _treshold)
            {
                _moveToPosition = null;
                return;
            }
            var xDifference = _moveToPosition.Value.x - currentPosition.x;
            _characterController.Move(xDifference, 0, false);
        }
    }
}
