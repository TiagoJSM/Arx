using Assets.Standard_Assets.QuestSystem.QuestStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.UI.Quest_Section.Scripts
{
    public class QuestSection : MonoBehaviour
    {
        [SerializeField]
        private QuestListManager _questListManager;
        [SerializeField]
        private QuestDescriptionManager _descriptionManager;

        public QuestLogComponent QuestLog { get; private set; }
        public Quest SelectedQuest { get { return _descriptionManager.Quest; } }

        public event OnQuestSelected OnSetActiveQuest;

        public void Initialize(QuestLogComponent questLog)
        {
            QuestLog = questLog;
            _questListManager.SetQuests(questLog);
            _questListManager.OnQuestSelected += OnQuestSelectedHandler;
        }

        public void SetActiveQuest()
        {
            if (OnSetActiveQuest != null)
            {
                OnSetActiveQuest(_descriptionManager.Quest);
            }
        }

        private void OnQuestSelectedHandler(Quest quest)
        {
            _descriptionManager.Quest = quest;
        }
    }
}
