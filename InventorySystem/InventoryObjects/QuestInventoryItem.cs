using CommonInterfaces.Inventory;
using InventorySystem.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventorySystem.InventoryObjects
{
    public class QuestInventoryItem : ExtendedInventoryItem
    {
        public override InventoryItemType ItemType
        {
            get
            {
                return InventoryItemType.QuestItem;
            }
        }
        public override bool CanUse()
        {
            return GetItemUseTrigger() != null;
        }

        public override void UseItem()
        {
            var trigger = GetItemUseTrigger();
            if(trigger!= null)
            {
                trigger.Use(Owner, this);
            }
        }

        private IItemUseTrigger GetItemUseTrigger()
        {
            var itemUseFinder = Owner.GetComponent<IItemUseFinderController>();

            foreach (var trigger in itemUseFinder.ItemUseTriggers)
            {
                if (trigger.ValidateUsage(Owner, this))
                {
                    return trigger;
                }
            }
            return null;
        }
    }
}
