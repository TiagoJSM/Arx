using CommonInterfaces.Inventory;
using InventorySystem.InventoryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventorySystem
{
    public class Inventory : IInventory
    {
        private Dictionary<Type, InventoryItems> _inventoryItems;
        private IItemOwner _owner;

        public Inventory(IItemOwner owner)
        {
            _inventoryItems = new Dictionary<Type, InventoryItems>();
            _owner = owner;
        }

        public bool AddItem(IInventoryItem item)
        {
            InventoryItems items;
            if(!_inventoryItems.ContainsKey(item.GetType()))
            {
                items = new InventoryItems();
                _inventoryItems.Add(item.GetType(), items);
            }
            else
            {
                items = _inventoryItems[item.GetType()];
            }
            item.Owner = _owner;
            return items.Add(item);
        }

        public bool RemoveItem(IInventoryItem item)
        {
            InventoryItems items;
            if (_inventoryItems.TryGetValue(item.GetType(), out items))
            {
                item.Owner = null;
                return items.Remove(item);
            }
            return false;
        }
    }
}
