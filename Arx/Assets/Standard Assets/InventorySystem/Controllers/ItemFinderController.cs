using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.InventorySystem.Controllers
{
    public class ItemFinderController : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _itemPickup;

        public OnInventoryItemFound OnInventoryItemFound;

        public void AssignItem(IInventoryItem item)
        {
            if (OnInventoryItemFound != null)
            {
                OnInventoryItemFound(item);
                _itemPickup.Play();
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            var itemPickable = other.gameObject.GetComponent<InventoryItemPickable>();
            if (itemPickable == null)
            {
                return;
            }
            if(OnInventoryItemFound != null)
            {
                OnInventoryItemFound(itemPickable.Item);
                itemPickable.PickUp();
                _itemPickup.Play();
            }
        }
    }
}
