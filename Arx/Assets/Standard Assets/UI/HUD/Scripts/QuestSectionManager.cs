using Assets.Standard_Assets.QuestSystem.QuestStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Standard_Assets.UI.HUD.Scripts
{
    public class QuestSectionManager : MonoBehaviour
    {
        private Quest _activeQuest;

        [SerializeField]
        private Text _title;
        [SerializeField]
        private GameObject _taskList;
        [SerializeField]
        private TaskItemManager _taskPrefab;

        public Quest ActiveQuest
        {
            get
            {
                return _activeQuest;
            }
            set
            {
                _activeQuest = value;
                HandleActiveQuest();
            }
        }

        private void HandleActiveQuest()
        {
            if(_activeQuest == null)
            {
                this.gameObject.SetActive(false);
                return;
            }

            RemoveCurrentSelectedQuest();

            this.gameObject.SetActive(true);
            _title.text = _activeQuest.questName;
            var tasks = _activeQuest.tasks;
            for(var idx = 0; idx < tasks.Count; idx++)
            {
                var prefab = Instantiate(_taskPrefab);
                prefab.Task = tasks[idx];
                prefab.transform.parent = _taskList.transform;
            }
        }

        private void RemoveCurrentSelectedQuest()
        {
            _taskList.transform.DetachChildren();
        }
    }
}
