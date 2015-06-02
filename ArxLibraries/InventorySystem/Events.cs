using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventorySystem
{
    public delegate void OnInventoryItemFound(IInventoryItem item);
}
