using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InventorySystem.InventoryItemUseValidations
{
    public abstract class InventoryItemUse : ScriptableObject
    {
        public abstract void Use(IItemOwner owner);
    }
}
