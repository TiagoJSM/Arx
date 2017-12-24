using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using CommonInterfaces;
using Assets.Standard_Assets.InventorySystem.InventoryObjects;
using Assets.Standard_Assets.InventorySystem.Controllers;
using Assets.Standard_Assets.Common;

namespace Assets.Standard_Assets.Loot
{
    public class Chest : MonoBehaviour, IInteractionTriggerController
    {
        private bool _open;

        [SerializeField]
        private InventoryItem _item;

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
            if (_open)
            {
                return;
            }

            var itemFinder = interactor.GetComponent<ItemFinderController>();

            if (itemFinder != null)
            {
                itemFinder.AssignItem(_item);
                _open = true;
                Destroy(this);
            }
        }

        public void StopInteraction()
        {
            
        }
    }
}
