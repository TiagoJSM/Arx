using Assets.Standard_Assets.QuestSystem.QuestStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Standard_Assets.UI.Quest_Section.Scripts
{
    public delegate void OnQuestSelected(Quest quest);

    public class QuestItemManager : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Text _text;
        [SerializeField]
        private Image _completion;

        private Quest _quest;

        public Quest Quest
        {
            get
            {
                return _quest;
            }
            set
            {
                _quest = value;
                _text.text = _quest.questName;
                _completion.color = _quest.QuestStatus == QuestStatus.Complete ? Color.green : Color.red;
            }
        }

        public event OnQuestSelected OnQuestSelected;

        public void OnPointerClick(PointerEventData eventData)
        {
            if(OnQuestSelected != null)
            {
                OnQuestSelected(Quest);
            }
        }
    }
}
