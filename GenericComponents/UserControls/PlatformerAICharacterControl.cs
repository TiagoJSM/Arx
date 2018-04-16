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
    public abstract class PlatformerAICharacterControl : MonoBehaviour, IPlatformerAICharacterControl
    {
        private Vector2? _moveToPosition;
        private float _treshold;

        protected Vector2? MoveToPosition
        {
            get
            {
                return _moveToPosition;
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
    }
}
