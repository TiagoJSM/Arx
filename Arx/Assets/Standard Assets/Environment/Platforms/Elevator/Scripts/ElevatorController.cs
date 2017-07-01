using CommonInterfaces.Controllers.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using CommonInterfaces;

namespace Assets.Standard_Assets.Environment.Platforms.Elevator.Scripts
{
    public class ElevatorController : MonoBehaviour, IInteractionTriggerController
    {
        [SerializeField]
        private Lift _lift;
        [SerializeField]
        private ElevatorPoints _point;
        [SerializeField]
        private bool _active = true;

        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
            }
        }

        public GameObject GameObject
        {
            get
            {
                return this.gameObject;
            }
        }

        public event OnInteract OnInteract;
        public event OnStopInteraction OnStopInteraction;

        public void Interact(GameObject interactor)
        {
            if(!_active)
            {
                return;
            }

            if (OnInteract != null)
            {
                OnInteract(interactor);
            }

            if (_point == ElevatorPoints.Point1)
            {
                _lift.GoToPoint1();
            }
            else
            {
                _lift.GoToPoint2();
            }
        }

        public void StopInteraction()
        {
        }
    }
}
