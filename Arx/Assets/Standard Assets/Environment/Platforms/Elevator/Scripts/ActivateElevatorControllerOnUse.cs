using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Platforms.Elevator.Scripts
{
    [RequireComponent(typeof(ElevatorController))]
    public class ActivateElevatorControllerOnUse : MonoBehaviour
    {
        private ElevatorController _controller;

        [SerializeField]
        private ElevatorController _deactivatedController;
        [SerializeField]
        private Lift _lift;

        private void Start()
        {
            _controller = GetComponent<ElevatorController>();
            _controller.OnInteract += Activate;
        }

        private void Activate(GameObject interactor)
        {
            _deactivatedController.Active = true;
            _lift.Active= true;
        }
    }
}
