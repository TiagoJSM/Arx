using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Controllers.Interaction
{
    public class InteractionTriggerController : MonoBehaviour, IInteractionTriggerController
    {
        private GameObject _interactor;

        public event OnInteract OnInteract;
        public event OnStopInteraction OnStopInteraction;

        public Canvas canvas;

        // Use this for initialization
        void Start()
        {
            canvas.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ColliderExitFromInteractionZone(Collider2D other)
        {
            if (other.gameObject == _interactor)
            {
                _interactor = null;
                StopInteraction();
            }
        }

        public void Interact(GameObject interactor)
        {
            _interactor = interactor;
            canvas.enabled = true;
            if (OnInteract != null)
            {
                OnInteract(interactor);
            }
        }

        public void StopInteraction()
        {
            canvas.enabled = false;
            if (OnStopInteraction != null)
            {
                OnStopInteraction();
            }
        }
    }
}
