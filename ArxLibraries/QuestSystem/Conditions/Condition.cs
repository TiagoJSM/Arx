using CommonInterfaces.Inventory;
using System;
using UnityEngine;

namespace QuestSystem.Conditions
{
    public abstract class Condition : ScriptableObject
	{
        public string name;
        public string description;

        public bool Complete { get; protected set; }

        public Condition()
        {
            description = string.Empty;
            name = string.Empty;
        }

        public abstract void Killed(GameObject obj);
        public abstract void InventoryItemAdded(IInventoryItem item);
        public abstract void InventoryItemRemoved(IInventoryItem item);
	}
}

