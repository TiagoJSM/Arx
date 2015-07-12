using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InventorySystem.InventoryItemUseValidations
{
    public abstract class UseValidation : ScriptableObject
    {
        public abstract bool CanUse(IItemOwner owner);
    }
}
