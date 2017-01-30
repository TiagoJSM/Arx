using Assets.Standard_Assets.QuestSystem.QuestStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem.Controllers
{
    public class SetStateOnQuestStatus : MonoBehaviour
    {
        private QuestLogComponent _questLog;

        [SerializeField]
        private bool _activate;
        [SerializeField]
        private GameObject _target;
        [SerializeField]
        private Quest _quest;
        [SerializeField]
        private QuestStatus _status;

        private void Awake()
        {
            _questLog = FindObjectOfType<QuestLogComponent>();
            InvokeRepeating("CheckQuestStatus", 0, 1);
        }

        private void CheckQuestStatus()
        {
            var quest = _questLog.GetQuest(_quest.questId);
            if (quest.QuestStatus == _status)
            {
                gameObject.SetActive(_activate);
                CancelInvoke("CheckQuestStatus");
            }
        }
    }
}
