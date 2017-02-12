using Assets.Standard_Assets.QuestSystem.QuestStructures;
using Assets.Standard_Assets.UI.HUD.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Standard_Assets.UI.Quest_Section.Scripts
{
    public class QuestDescriptionManager : MonoBehaviour
    {
        private Quest _quest;

        [SerializeField]
        private Text _title;
        [SerializeField]
        private Text _description;
        [SerializeField]
        private GameObject _taskList;
        [SerializeField]
        private TaskItemManager _taskPrefab;
        [SerializeField]
        private Button _setActiveQuestButton;

        public Quest Quest
        {
            get
            {
                return _quest;
            }
            set
            {
                _quest = value;
                ShowQuest(_quest);
                _setActiveQuestButton.interactable = true;
            }
        }

        private void Awake()
        {
            _setActiveQuestButton.interactable = false;
        }

        private void ShowQuest(Quest quest)
        {
            _title.text = quest.questName;
            _description.text = quest.description;

            RemoveTasks();

            var tasks = quest.tasks;
            for (var idx = 0; idx < tasks.Count; idx++)
            {
                var taskManager = Instantiate(_taskPrefab);
                taskManager.Task = tasks[idx];
                taskManager.transform.SetParent(_taskList.transform, false);
            }
        }

        private void RemoveTasks()
        {
            var taskItems = _taskList.GetComponents<TaskItemManager>();
            for (var idx = 0; idx < taskItems.Length; idx++)
            {
                Destroy(taskItems[idx].gameObject);
            }
            _taskList.transform.DetachChildren();
        }
    }
}
