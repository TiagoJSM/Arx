using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventorySystem.InventoryItems
{
    public class IventoryItems
    {
        private List<IInventoryItem> _items;

        public IventoryItems()
        {
            _items = new List<IInventoryItem>();
        }
    }
}
