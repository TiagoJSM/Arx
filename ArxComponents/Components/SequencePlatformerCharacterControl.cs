using CommonInterfaces.Enums;
using GenericComponents.Controllers.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components
{
    [RequireComponent(typeof(PlatformerCharacterController))]
    public class SequencePlatformerCharacterControl : MonoBehaviour
    {
        private PlatformerCharacterController _characterController;

        public void Move(Direction direction)
        {
            _characterController.Move(direction == Direction.Left ? -1 : 1, 0, false);
        }

        void Start()
        {
            _characterController = this.GetComponent<PlatformerCharacterController>();
        }
    }
}
