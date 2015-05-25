using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
	public class QuestLog
	{
		private List<Quest> _quests;
		private IQuestSubscriber _subscriber;

		public QuestLog (IQuestSubscriber subscriber)
		{
			_quests = new List<Quest> ();
			_subscriber = subscriber;

			_subscriber.OnKill += OnKillHandler;
			_subscriber.OnInventoryItemAdd += OnInventoryItemAddHandler;
			_subscriber.OnInventoryItemRemove += OnInventoryItemRemoveHandler;
		}

		private void OnKillHandler(GameObject obj)
		{
			foreach (var quest in _quests) 
			{
				quest.Killed (obj);
			}
		}

		private void OnInventoryItemAddHandler(GameObject obj)
		{
			foreach (var quest in _quests) 
			{
				quest.InventoryItemAdded (obj);
			}
		}

		private void OnInventoryItemRemoveHandler(GameObject obj)
		{
			foreach (var quest in _quests) 
			{
				quest.InventoryItemRemoved (obj);
			}
		}
	}
}

