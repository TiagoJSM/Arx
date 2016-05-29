using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InventorySystem.ItemCallbacks
{
    public abstract class InventoryItemUse : ScriptableObject
    {
        public abstract bool Use(IItemOwner owner, IInventoryItem item);
    }
}
