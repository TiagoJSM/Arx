using CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Common
{
    public interface IInteractionTriggerController
    {
        GameObject GameObject { get; }

        event OnInteract OnInteract;
        event OnStopInteraction OnStopInteraction;

        void Interact(GameObject interactor);
        void StopInteraction();
    }
}
