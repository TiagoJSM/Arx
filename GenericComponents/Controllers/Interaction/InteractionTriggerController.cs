using CommonInterfaces;
using CommonInterfaces.Controllers.Interaction;
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

        public void ColliderExitFromInteractionZone(Collider2D other)
        {
            if (other.gameObject == _interactor)
            {
                StopInteraction();
            }
        }

        public void Interact(GameObject interactor)
        {
            _interactor = interactor;
            if (OnInteract != null)
            {
                OnInteract(interactor);
            }
        }

        public void StopInteraction()
        {
            _interactor = null;
            if (OnStopInteraction != null)
            {
                OnStopInteraction();
            }
        }
    }
}
