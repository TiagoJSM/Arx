using GenericComponents.Controllers.Characters;
using GenericComponents.Controllers.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components.UserControls
{
    [RequireComponent(typeof(MainPlatformerController))]
    public class PlatformerCharacterUserControl : MonoBehaviour
    {
        private MainPlatformerController _characterController;
        private bool _jump;

        public InteractionFinderController interactionController;

        public MainPlatformerController PlatformerCharacterController
        {
            get
            {
                return _characterController;
            }
        }

        private void Awake()
        {
            _characterController = GetComponent<MainPlatformerController>();
        }

        private void Update()
        {
            if (!_jump)
            {
                _jump = Input.GetButtonDown("Jump");
            }

            if (interactionController.InteractionTriggerController != null && Input.GetButtonDown("Interact"))
            {
                interactionController.InteractionTriggerController.Interact(this.gameObject);
            }
        }

        private void FixedUpdate()
        {
            // Read the inputs.
            //bool crouch = Input.GetKey(KeyCode.LeftControl);
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            // Pass all parameters to the character control script.
            _characterController.Move(horizontal, vertical, _jump);
            _jump = false;
            if (Input.GetButtonDown("Fire1"))
            {
                _characterController.LightAttack();
            }

        }
    }
}
