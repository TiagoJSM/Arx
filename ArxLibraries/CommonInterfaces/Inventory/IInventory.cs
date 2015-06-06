using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonInterfaces.Inventory
{
    public interface IInventory
    {
        bool AddItem(IInventoryItem item);
        bool RemoveItem(IInventoryItem item);
    }
}
