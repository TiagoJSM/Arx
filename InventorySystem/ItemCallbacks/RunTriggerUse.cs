using CommonInterfaces.Inventory;
using InventorySystem.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventorySystem.ItemCallbacks
{
    public class RunTriggerUse : InventoryItemUse
    {
        public override bool Use(IItemOwner owner, IInventoryItem item)
        {
            var itemUseFinder = owner.GetComponent<ItemUseFinderController>();

            foreach (var trigger in itemUseFinder.ItemUseTriggers)
            {
                if (trigger.ValidateUsage(owner, item))
                {
                    trigger.Use(owner, item);
                    return true;
                }
            }

            return false;
        }
    }
}
