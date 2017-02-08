using Assets.Standard_Assets.InventorySystem.InventoryObjects;
using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.InventorySystem
{
    public class Inventory : IInventory
    {
        private Dictionary<string, InventoryItems> _inventoryItems;
        private IItemOwner _owner;

        public IEnumerable<InventoryItems> InventoryItems
        {
            get
            {
                return _inventoryItems.Values;
            }
        }

        public Inventory(IItemOwner owner)
        {
            _inventoryItems = new Dictionary<string, InventoryItems>();
            _owner = owner;
        }

        public bool AddItem(IInventoryItem item)
        {
            InventoryItems items;
            if(!_inventoryItems.ContainsKey(item.Id))
            {
                items = new InventoryItems()
                {
                    Item = item,
                    Count = 1
                };
                _inventoryItems.Add(item.Id, items);
            }
            else
            {
                items = _inventoryItems[item.Id];
                items.Count++;
            }
            item.Owner = _owner;
            return true;
        }

        public bool RemoveItem(IInventoryItem item)
        {
            InventoryItems items;
            if (!_inventoryItems.TryGetValue(item.Id, out items))
            {
                return false;
            }
            items.Count--;
            if(items.Count <= 0)
            {
                _inventoryItems.Remove(item.Id);
            }
            return true;
        }

        public void SetItems(InventoryItems[] items)
        {
            _inventoryItems.Clear();
            for(var idx = 0; idx < items.Length; idx++)
            {
                var descriptor = items[idx];
                _inventoryItems.Add(descriptor.Item.Id, descriptor);
                descriptor.Item.Owner = _owner;
            }
        }
    }
}
