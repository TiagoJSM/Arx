using CommonInterfaces.Inventory;
using InventorySystem.InventoryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventorySystem
{
    public class Inventory
    {
        private Dictionary<Type, InventoryItems> _inventoryItems;

        public Inventory()
        {
            _inventoryItems = new Dictionary<Type, InventoryItems>();
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
            return items.Add(item);
        }

        public bool RemoveItem(IInventoryItem item)
        {
            InventoryItems items;
            if (_inventoryItems.TryGetValue(item.GetType(), out items))
            {
                return items.Remove(item);
            }
            return false;
        }
    }
}
