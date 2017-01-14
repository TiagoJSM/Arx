using Assets.Standard_Assets.QuestSystem.Attributes;
using Assets.Standard_Assets.QuestSystem.EditorProperties;
using Assets.Standard_Assets.QuestSystem.QuestStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem.Controllers
{
    public class ToggleOnTaskActive : MonoBehaviour
    {
        private QuestLogComponent _questLog;

        [SerializeField]
        private bool activate;
        [SerializeField]
        private GameObject _target;
        [TaskSelector]
        [SerializeField]
        private TaskSelector _taskSelector;

        private void Awake()
        {
            _questLog = FindObjectOfType<QuestLogComponent>();
            InvokeRepeating("CheckQuestStatus", 0, 1);
        }

        private void CheckQuestStatus()
        {
            var quest = _questLog.GetQuest(_taskSelector.Quest.questId);
            if(quest.QuestStatus == QuestStatus.Active)
            {
                gameObject.SetActive(activate);
            }
            else
            {
                gameObject.SetActive(!activate);
            }
        }
    }
}
