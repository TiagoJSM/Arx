using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using CommonInterfaces.Inventory;
using QuestSystem.Conditions;

namespace QuestSystem.QuestStructures
{
    [Serializable]
	public class Quest : ScriptableObject, IEquatable<Quest>
	{
        public bool Active { get; set; }

		public string name;
		public string description;
		public List<Condition> conditions;

        public QuestStatus QuestStatus 
        {
            get
            {
                if (!Active)
                {
                    return QuestStatus.Inactive;
                }
                if (conditions.All(c => c.Complete))
                {
                    return QuestStatus.Complete;
                }
                return QuestStatus.Active;
            }
        }

        public Quest()
        {
            conditions = new List<Condition>();
        }

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

		private IEnumerable<Condition> GetIncompleteConditions()
		{
			return conditions.Where(c => !c.Complete);
		}

        public override bool Equals(object obj)
        {
            var quest = obj as Quest;
            if (quest == null)
            {
                return base.Equals(obj);
            }
            return Equals(quest);
        }

        public bool Equals(Quest other)
        {
            return this.name.Equals(other.name);
        }
    }
}

