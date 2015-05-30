using CommonInterfaces.Inventory;
using InventorySystem.InventoryItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventorySystem
{
    public class Inventory
    {
        private Dictionary<Type, IventoryItems> _inventoryItems;

        public Inventory()
        {
            _inventoryItems = new Dictionary<Type, IventoryItems>();
        }

        public void Add(IInventoryItem item)
        {

        }
    }
}
