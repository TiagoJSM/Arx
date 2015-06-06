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
            var item = other.gameObject.GetComponent<IInventoryItem>();
            if (item == null)
            {
                return;
            }
            if(OnInventoryItemFound != null)
            {
                OnInventoryItemFound(item);
            }
        }
    }
}
