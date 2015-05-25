using System;
using UnityEngine;

namespace QuestSystem
{
	public interface IQuestSubscriber
	{
		event OnKill OnKill;
		event OnInventoryAdd OnInventoryItemAdd;
		event OnInventoryRemove OnInventoryItemRemove;
	}
}

