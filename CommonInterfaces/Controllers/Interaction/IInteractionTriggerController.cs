using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CommonInterfaces.Controllers.Interaction
{
    public interface IInteractionTriggerController
    {
        event OnInteract OnInteract;
        event OnStopInteraction OnStopInteraction;
        GameObject GameObject { get; }

        void Interact(GameObject interactor);
        void StopInteraction();
    }
}
