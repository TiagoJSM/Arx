using CommonInterfaces.Controllers.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using CommonInterfaces;
using Assets.Standard_Assets.InventorySystem;
using Assets.Standard_Assets.InventorySystem.InventoryObjects;
using GenericComponents.Controllers.Interaction;
using Assets.Standard_Assets.UI.Speech.Scripts;

namespace Assets.Standard_Assets._2D.Scripts.Interaction
{
    public class OpenPathWithKeyOnInteract : MonoBehaviour, IInteractionTriggerController
    {
        private bool _isUnlocked;

        [SerializeField]
        private Collider2D _collider;
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private string _animationOnOpen = "Open";
        [SerializeField]
        private InventoryItem _usableItem;
        [SerializeField]
        private LocalizedTexts _conversation;
        [SerializeField]
        private SpeechController _speechController;

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
            if (_isUnlocked)
            {
                return;
            }
            var hasItem = interactor.GetComponent<InventoryComponent>().Inventory.HasItem(_usableItem);
            if(!hasItem)
            {
                _speechController.Say("locked");
                return;
            }
            _collider.enabled = false;

            if (_animator != null)
            {
                _animator.Play(_animationOnOpen);
            }
            _speechController.Say("unlocked");
            _isUnlocked = true;
        }

        public void StopInteraction()
        {
            _speechController.Close();
        }
    }
}
