using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using CommonInterfaces.Inventory;
using QuestSystem.Conditions;

namespace QuestSystem.QuestStructures
{
	public class Quest
	{
		public string name;
		public string description;
		public List<ICondition> conditions;

		//public QuestStatus QuestStatus{ get; }

		public void Killed(GameObject obj)
		{
			foreach (var condition in GetIncompleteConditions()) 
			{
				condition.Killed (obj);
			}
		}

		public void InventoryItemAdded(IInventoryItem item)
		{
			foreach (var condition in GetIncompleteConditions()) 
			{
				condition.InventoryItemAdded (item);
			}
		}

        public void InventoryItemRemoved(IInventoryItem item)
		{
			foreach (var condition in GetIncompleteConditions()) 
			{
                condition.InventoryItemRemoved(item);
			}
		}

		private IEnumerable<ICondition> GetIncompleteConditions()
		{
			return conditions.Where(c => !c.Complete);
		}
	}
}

