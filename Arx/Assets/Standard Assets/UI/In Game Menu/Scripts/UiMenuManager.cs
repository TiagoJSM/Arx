using Assets.Standard_Assets.InventorySystem;
using Assets.Standard_Assets.QuestSystem.QuestStructures;
using Assets.Standard_Assets.UI.Game_Section.Scripts;
using Assets.Standard_Assets.UI.InventorySection.Scripts;
using Assets.Standard_Assets.UI.Quest_Section.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Standard_Assets.UI.In_Game_Menu.Scripts
{
    public class UiMenuManager : MonoBehaviour
    {
        [SerializeField]
        private Toggle _startingToggle;
        [SerializeField]
        private EventSystem _eventSystem;
        [SerializeField]
        private GameSectionManager _gameSection;

        private GameSectionManager _gameSectionInstance;
        private GameObject _currentSection;
        private InventorySectionManager _inventoryInstance;
        private QuestSection _questInstance;

        public InventorySectionManager inventorySection;
        public QuestSection questSection;

        public InventoryComponent inventoryComponent;
        public QuestLogComponent questLogComponent;

        public event OnQuestSelected OnSetActiveQuest;

        public void SetGameSection(bool toggle)
        {
            if (toggle)
            {
                SetSection(_gameSectionInstance.gameObject);
            }
        }

        public void SetInventorySection(bool toggle)
        {
            if (toggle)
            {
                SetSection(_inventoryInstance.gameObject);
                var items = _inventoryInstance.listManager.InventoryItems;
                if (items.Any())
                {
                    _eventSystem.SetSelectedGameObject(items.First().gameObject);
                }
            }
        }

        public void SetQuestSection(bool toggle)
        {
            if (toggle)
            {
                SetSection(_questInstance.gameObject);
                var quests = _questInstance.QuestItems;
                if (quests.Any())
                {
                    _eventSystem.SetSelectedGameObject(quests.First().gameObject);
                }
            }
        }

        private void Start()
        {
            _gameSectionInstance = Instantiate(_gameSection);
            _inventoryInstance = Instantiate(inventorySection);
            _questInstance = Instantiate(questSection);

            _gameSectionInstance.transform.SetParent(this.transform, false);
            _inventoryInstance.transform.SetParent(this.transform, false);
            _questInstance.transform.SetParent(this.transform, false);

            if (inventoryComponent != null)
            {
                _inventoryInstance.Initialize(inventoryComponent);
            }
            if (questLogComponent != null)
            {
                _questInstance.Initialize(questLogComponent);
            }

            _gameSectionInstance.gameObject.SetActive(false);
            _inventoryInstance.gameObject.SetActive(false);
            _questInstance.gameObject.SetActive(false);

            _questInstance.OnSetActiveQuest += OnSetActiveQuestHandler;

            _startingToggle.isOn = true;
        }

        private void SetSection(GameObject section)
        {
            if (_currentSection != null)
            {
                _currentSection.SetActive(false);
            }

            _currentSection = section;
            _currentSection.SetActive(true);
        }

        private void OnSetActiveQuestHandler(Quest quest)
        {
            if (OnSetActiveQuest != null)
            {
                OnSetActiveQuest(quest);
            }
        }
    }
}
