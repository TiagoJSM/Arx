using GenericComponents.Controllers.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using Utils;

namespace GenericComponents.UserControls
{
    [RequireComponent(typeof(PlatformerCharacterController))]
    public class PlatformerAICharacterControl : MonoBehaviour
    {
        private PlatformerCharacterController _characterController;

        public bool MoveDirectlyTo(Vector2 position, float treshold)
        {
            var currentPosition = this.transform.position.ToVector2();
            var distance = Vector2.Distance(currentPosition, position);
            if(distance < treshold)
            {
                return true;
            }
            var xDifference = currentPosition.x - position.x;
            _characterController.Move(xDifference, 0, false);
            return false;
        }

        void Start()
        {
            _characterController = GetComponent<PlatformerCharacterController>();
        }
    }
}
