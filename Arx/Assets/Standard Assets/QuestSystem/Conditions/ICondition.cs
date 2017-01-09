using CommonInterfaces.Inventory;
using System;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem.Conditions
{
    public interface ICondition
	{
        string ConditionName { get; set; }
        string Description { get; set; }

        bool Complete { get; }

        void Killed(GameObject obj);
        void InventoryItemAdded(IInventoryItem item);
        void InventoryItemRemoved(IInventoryItem item);
	}
}

