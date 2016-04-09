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

namespace GenericComponents.UserControls
{
    [RequireComponent(typeof(PlatformerCharacterController))]
    public abstract class PlatformerAICharacterControl : MonoBehaviour, IPlatformerAICharacterControl
    {
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
