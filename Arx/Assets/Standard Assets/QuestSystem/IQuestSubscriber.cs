using Assets.Standard_Assets.QuestSystem.QuestStructures;
using System;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem
{
	public interface IQuestSubscriber
	{
		event OnKill OnKill;
		event OnInventoryAdd OnInventoryItemAdd;
		event OnInventoryRemove OnInventoryItemRemove;

        void AssignQuest(Quest quest);
        bool HasQuestActive(Quest quest);
        Quest GetQuest(string name);
	}
}

