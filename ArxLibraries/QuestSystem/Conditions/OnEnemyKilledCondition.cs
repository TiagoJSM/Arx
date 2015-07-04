using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace QuestSystem.Conditions
{
    public class OnEnemyKilledCondition : ICondition
    {
        public string Description { get; private set; }
        public bool Complete { get; private set; }

        public void Killed(GameObject obj)
        {

        }

        public void InventoryItemAdded(IInventoryItem item)
        {

        }

        public void InventoryItemRemoved(IInventoryItem item)
        {

        }
    }
}
