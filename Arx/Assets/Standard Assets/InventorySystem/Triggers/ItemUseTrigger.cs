using Assets.Standard_Assets.InventorySystem.InventoryObjects;
using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.InventorySystem.Triggers
{
    public abstract class ItemUseTrigger : MonoBehaviour, IItemUseTrigger
    {
        [SerializeField]
        private InventoryItem _usableItem;

        public abstract bool Use(IItemOwner owner, IInventoryItem item);

        public bool ValidateUsage(IItemOwner owner, IInventoryItem item)
        {
            return item.Id == _usableItem.Id;
        }
    }
}
