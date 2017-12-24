using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonInterfaces;
using UnityEngine;

namespace AnimatorSequencer.Zones
{
    public class PlaySequenceOnInteraction : MonoBehaviour//, IInteractionTriggerController
    {
        public event OnInteract OnInteract;
        public event OnStopInteraction OnStopInteraction;

        public bool playOnlyOnce = true;
        public bool played;
        public AnimationSequenceBehaviour animationSequence;

        public GameObject GameObject
        {
            get
            {
                return this.gameObject;
            }
        }

        public void Interact(GameObject interactor)
        {
            if(playOnlyOnce && played)
            {
                return;
            }

            animationSequence.Run();
            played = true;
        }

        public void StopInteraction()
        {
        }
    }
}
