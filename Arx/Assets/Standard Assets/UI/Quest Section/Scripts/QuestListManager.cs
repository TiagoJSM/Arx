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
        [SerializeField]
        private QuestItemManager _questItemPrefab;
        [SerializeField]
        private GameObject _questListContent;

        public event OnQuestSelected OnQuestSelected;

        public void SetQuests(QuestLogComponent questLog)
        {
            RemoveAllChildren();
            var quests = questLog.GetQuests();
            for(var idx = 0; idx < quests.Length; idx++)
            {
                var itemManager = Instantiate(_questItemPrefab);
                itemManager.OnQuestSelected += OnQuestSelectedHandler;
                itemManager.Quest = quests[idx];
                itemManager.transform.parent = _questListContent.transform;
            }
        }

        private void RemoveAllChildren()
        {
            var transform = _questListContent.transform;
            for(var idx = 0; idx < transform.childCount; idx++)
            {
                var child = transform.GetChild(idx);
                var itemManager = child.GetComponent<QuestItemManager>();
                itemManager.OnQuestSelected -= OnQuestSelectedHandler;
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
