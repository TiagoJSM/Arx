using System;
using UnityEngine;

namespace QuestSystem
{
	public interface ICondition
	{
		string Description { get; }
		bool Complete { get; }
		void Killed (GameObject obj);
		void InventoryItemAdded(GameObject obj);
		void InventoryItemRemoved(GameObject obj);
	}
}

