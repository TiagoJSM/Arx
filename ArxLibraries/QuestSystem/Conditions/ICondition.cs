using CommonInterfaces.Inventory;
using System;
using UnityEngine;

namespace QuestSystem.Conditions
{
	public interface ICondition
	{
		string Description { get; }
		bool Complete { get; }
		void Killed (GameObject obj);
		void InventoryItemAdded(IInventoryItem item);
        void InventoryItemRemoved(IInventoryItem item);
	}
}

