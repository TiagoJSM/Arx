using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonInterfaces.Inventory
{
    public interface IItemUseTrigger
    {
        bool ValidateUsage(IItemOwner owner, IInventoryItem item);
        bool Use(IItemOwner owner, IInventoryItem item);
    }
}
