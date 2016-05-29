using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InventorySystem.Controllers
{
    public class ItemUseFinderController : MonoBehaviour
    {
        public List<IItemUseTrigger> ItemUseTriggers { get; private set; }

        public ItemUseFinderController()
        {
            ItemUseTriggers = new List<IItemUseTrigger>();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            var item = other.gameObject.GetComponent<IItemUseTrigger>();
            if (item == null)
            {
                return;
            }
            ItemUseTriggers.Add(item);
        }

        void OnTriggerExit2D(Collider2D other)
        {
            var item = other.gameObject.GetComponent<IItemUseTrigger>();
            if (item == null)
            {
                return;
            }
            ItemUseTriggers.Remove(item);
        }
    }
}
