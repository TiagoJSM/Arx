using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Controllers.Interaction
{
    public class InteractionZoneController : MonoBehaviour
    {
        public InteractionTriggerController triggerController;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerExit2D(Collider2D other)
        {
            triggerController.ColliderExitFromInteractionZone(other);
        }
    }
}
