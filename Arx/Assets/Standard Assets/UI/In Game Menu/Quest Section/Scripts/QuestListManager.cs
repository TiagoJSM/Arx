using Assets.Standard_Assets.QuestSystem.QuestStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.UI.Quest_Section.Scripts
{
    public class QuestListManager : MonoBehaviour
    {
        private Quest[] _quests;
        [SerializeField]
        private QuestItemManager _questItemPrefab;
        [SerializeField]
        private GameObject _questListContent;

        public QuestItemManager[] QuestItems
        {
            get
            {
                return _questListContent.GetComponents<QuestItemManager>();
            }
        }

        public event OnQuestSelected OnQuestSelected;

        public void SetQuests(QuestLogComponent questLog)
        {
            _quests = questLog.GetQuests();
        }

        private void OnEnable()
        {
            if(_quests == null)
            {
                return;
            }
            var activeQuests = _quests.Where(quest => quest.QuestStatus != QuestStatus.Inactive).ToArray();
            for (var idx = 0; idx < activeQuests.Length; idx++)
            {
                var itemManager = Instantiate(_questItemPrefab);
                itemManager.OnQuestSelected += OnQuestSelectedHandler;
                itemManager.Quest = activeQuests[idx];
                itemManager.transform.SetParent(_questListContent.transform, false);
            }
        }

        private void OnDisable()
        {
            RemoveAllChildren();
        }

        private void RemoveAllChildren()
        {
            var transform = _questListContent.transform;
            for(var idx = 0; idx < transform.childCount; idx++)
            {
                var child = transform.GetChild(idx);
                var itemManager = child.GetComponent<QuestItemManager>();
                itemManager.OnQuestSelected -= OnQuestSelectedHandler;
                Destroy(itemManager.gameObject);
            }
            transform.DetachChildren();
        }

        private void OnQuestSelectedHandler(Quest quest)
        {
            if(OnQuestSelected != null)
            {
                OnQuestSelected(quest);
            }
        }
    }
}
