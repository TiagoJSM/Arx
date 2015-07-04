using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonInterfaces.Controllers.Interaction
{
    public interface IInteractionTriggerController
    {
        event OnInteract OnInteract;
        event OnStopInteraction OnStopInteraction;

        void StopInteraction();
    }
}
