using Assets.Standard_Assets.QuestSystem.QuestStructures;
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
        }
    }
}
