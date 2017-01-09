using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem.Conditions
{
    [Serializable]
    public class OnEnemyKilledCondition : ICondition
    {
        public int killCount { get; set; }

        public string ConditionName { get; set; }

        public string Description { get; set; }

        public bool Complete { get; set; }

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
