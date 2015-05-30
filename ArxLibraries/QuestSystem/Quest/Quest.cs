using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
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

		public void InventoryItemAdded(GameObject obj)
		{
			foreach (var condition in GetIncompleteConditions()) 
			{
				condition.InventoryItemAdded (obj);
			}
		}

		public void InventoryItemRemoved(GameObject obj)
		{
			foreach (var condition in GetIncompleteConditions()) 
			{
				condition.InventoryItemRemoved (obj);
			}
		}

		private IEnumerable<ICondition> GetIncompleteConditions()
		{
			return conditions.Where(c => !c.Complete);
		}
	}
}

