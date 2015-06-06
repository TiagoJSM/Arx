using QuestSystem.QuestStructures;
using System;
using UnityEngine;

namespace QuestSystem
{
	public interface IQuestSubscriber
	{
		event OnKill OnKill;
		event OnInventoryAdd OnInventoryItemAdd;
		event OnInventoryRemove OnInventoryItemRemove;

        void AssignQuest(Quest quest);
        bool HasQuest(Quest quest);
	}
}

