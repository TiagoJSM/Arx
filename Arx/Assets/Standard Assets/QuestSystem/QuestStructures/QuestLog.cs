using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem.QuestStructures
{
	public class QuestLog
	{
		private Dictionary<string, Quest> _quests;
		private IQuestSubscriber _subscriber;

		public QuestLog (IQuestSubscriber subscriber)
		{
            _quests = new Dictionary<string, Quest>();
			_subscriber = subscriber;

			/*_subscriber.OnKill += OnKillHandler;
            _subscriber.OnInventoryItemAdd += OnInventoryItemAddHandler;
			_subscriber.OnInventoryItemRemove += OnInventoryItemRemoveHandler;*/
		}

        public void AssignQuest(Quest quest)
        {
            if (HasQuest(quest))
            {
                return;
            }
            _quests.Add(quest.name, quest);
        }

        public bool HasQuest(Quest quest)
        {
            return _quests.ContainsKey(quest.name);
        }

        public Quest GetQuest(string name)
        {
            Quest quest;
            if (_quests.TryGetValue(name, out quest))
            {
                return quest;
            }
            return null;
        }

		/*private void OnKillHandler(GameObject obj)
		{
			foreach (var quest in _quests.Values) 
			{
				quest.Killed (obj);
			}
		}

        private void OnInventoryItemAddHandler(IInventoryItem item)
		{
            foreach (var quest in _quests.Values) 
			{
                quest.InventoryItemAdded(item);
			}
		}

        private void OnInventoryItemRemoveHandler(IInventoryItem item)
		{
            foreach (var quest in _quests.Values) 
			{
                quest.InventoryItemRemoved(item);
			}
		}*/
	}
}

