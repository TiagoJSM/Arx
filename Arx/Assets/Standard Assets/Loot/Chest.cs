using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using CommonInterfaces;
using Assets.Standard_Assets.InventorySystem.InventoryObjects;
using Assets.Standard_Assets.InventorySystem.Controllers;
using Assets.Standard_Assets.Common;
using Assets.Standard_Assets._2D.Scripts.Game_State;

namespace Assets.Standard_Assets.Loot
{
    [RequireComponent(typeof(UniqueId))]
    public class Chest : MonoBehaviour, IInteractionTriggerController
    {
        private UniqueId _id;
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
                var openChests = ArxSaveData.Current.GameState.OpenChests;
                openChests.Add(_id.Id);
                Destroy(this);
            }
        }

        public void StopInteraction()
        {
            
        }

        private void Awake()
        {
            _id = GetComponent<UniqueId>();
            var openChests = ArxSaveData.Current.GameState.OpenChests;
            _open = openChests.Contains(_id.Id);
        }
    }
}
