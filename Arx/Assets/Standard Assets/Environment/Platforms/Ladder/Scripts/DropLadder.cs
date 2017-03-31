using CommonInterfaces.Controllers.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using CommonInterfaces;

namespace Assets.Standard_Assets.Environment.Platforms.Ladder.Scripts
{
    public class DropLadder : MonoBehaviour, IInteractionTriggerController
    {
        private bool _drop;

        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private Collider2D _ladderCollider;
        [SerializeField]
        private Collider2D _trigger;

        public GameObject GameObject
        {
            get
            {
                return gameObject;
            }
        }

        public event OnInteract OnInteract;
        public event OnStopInteraction OnStopInteraction;

        public void Interact(GameObject interactor)
        {
            if (!_drop)
            {
                _drop = true;
                _ladderCollider.enabled = true;
                _trigger.enabled = false;
                _animator.enabled = true;
            }
        }

        public void StopInteraction()
        {
        }
    }
}
