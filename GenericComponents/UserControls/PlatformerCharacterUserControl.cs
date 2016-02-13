using UnityEngine;
using System.Collections;
using CommonInterfaces.Inventory;
using GenericComponents.Controllers.Interaction;
using GenericComponents.Controllers.Characters;

namespace GenericComponents.UserControls
{
    [RequireComponent(typeof(PlatformerCharacterController))]
    public class PlatformerCharacterUserControl : MonoBehaviour
    {
        private PlatformerCharacterController _characterController;
        private bool _jump;

        public InteractionFinderController interactionController;

        private void Awake()
        {
            _characterController = GetComponent<PlatformerCharacterController>();
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
        }
    }
}
