using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem.QuestStructures
{
    public class QuestLogComponent : MonoBehaviour
    {
        private QuestDatabase _instanciatedQuests;

        [SerializeField]
        private QuestDatabase _quests;
        //private QuestLog _questLog;
        
        public Quest GetQuest(string id)
        {
            return _instanciatedQuests.GetQuest(id);
        }

        public bool HasQuestActive(string id)
        {
            var quest = GetQuest(id);
            return quest.QuestStatus == QuestStatus.Active;
        }

        public bool HasQuestActive(Quest quest)
        {
            return HasQuestActive(quest.questId);
        }

        public void GiveQuest(Quest quest)
        {
            quest = _instanciatedQuests.GetQuest(quest.questId);
            quest.Active = true;
        }

        public Quest[] GetQuests()
        {
            return _instanciatedQuests.Quests;
        }

        private void Awake()
        {
            _instanciatedQuests = _quests.Clone();
        }

        private void Start()
        {
            var subscriber = this.gameObject.GetComponent<IQuestSubscriber>();
            //_questLog = new QuestLog(subscriber);
        }
    }
}
