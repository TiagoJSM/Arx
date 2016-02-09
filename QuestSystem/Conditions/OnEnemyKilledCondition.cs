using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace QuestSystem.Conditions
{
    public class OnEnemyKilledCondition : Condition
    {
        public int killCount;

        public override void Killed(GameObject obj)
        {

        }

        public override void InventoryItemAdded(IInventoryItem item)
        {

        }

        public override void InventoryItemRemoved(IInventoryItem item)
        {

        }
    }
}
