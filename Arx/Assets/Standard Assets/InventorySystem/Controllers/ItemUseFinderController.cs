using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.InventorySystem.Controllers
{
    public class ItemUseFinderController : MonoBehaviour, IItemUseFinderController
    {
        private List<IItemUseTrigger> _itemUseTriggers;
        public IEnumerable<IItemUseTrigger> ItemUseTriggers { get { return _itemUseTriggers; } }

        public ItemUseFinderController()
        {
            _itemUseTriggers = new List<IItemUseTrigger>();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            var item = other.gameObject.GetComponent<IItemUseTrigger>();
            if (item == null)
            {
                return;
            }
            _itemUseTriggers.Add(item);
        }

        void OnTriggerExit2D(Collider2D other)
        {
            var item = other.gameObject.GetComponent<IItemUseTrigger>();
            if (item == null)
            {
                return;
            }
            _itemUseTriggers.Remove(item);
        }
    }
}
