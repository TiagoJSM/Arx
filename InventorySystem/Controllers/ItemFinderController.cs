using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InventorySystem.Controllers
{
    public class ItemFinderController : MonoBehaviour
    {
        public OnInventoryItemFound OnInventoryItemFound;

        void OnTriggerEnter2D(Collider2D other)
        {
            var itemPickable = other.gameObject.GetComponent<InventoryItemPickable>();
            if (itemPickable == null)
            {
                return;
            }
            if(OnInventoryItemFound != null)
            {
                OnInventoryItemFound(itemPickable.item);
            }
        }
    }
}
