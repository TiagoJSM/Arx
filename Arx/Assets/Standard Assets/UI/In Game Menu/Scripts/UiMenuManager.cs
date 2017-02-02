using Assets.Standard_Assets.InventorySystem;
using Assets.Standard_Assets.QuestSystem.QuestStructures;
using Assets.Standard_Assets.UI.InventorySection.Scripts;
using Assets.Standard_Assets.UI.Quest_Section.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.UI.In_Game_Menu.Scripts
{
    public class UiMenuManager : MonoBehaviour
    {
        private GameObject _currentSection;
        private InventorySectionManager _inventoryInstance;
        private QuestSection _questInstance;

        public InventorySectionManager inventorySection;
        public QuestSection questSection;

        public InventoryComponent inventoryComponent;
        public QuestLogComponent questLogComponent;

        public event OnQuestSelected OnSetActiveQuest;

        public void SetInventorySection(bool toggle)
        {
            if (toggle)
            {
                SetSection(_inventoryInstance.gameObject);
            }
        }

        public void SetQuestSection(bool toggle)
        {
            if (toggle)
            {
                SetSection(_questInstance.gameObject);
            }
        }

        private void Start()
        {
            _inventoryInstance = Instantiate(inventorySection);
            _questInstance = Instantiate(questSection);

            _inventoryInstance.transform.SetParent(this.transform, false);
            _questInstance.transform.SetParent(this.transform, false);

            _inventoryInstance.Initialize(inventoryComponent);
            _questInstance.Initialize(questLogComponent);

            _inventoryInstance.gameObject.SetActive(false);
            _questInstance.gameObject.SetActive(false);

            _questInstance.OnSetActiveQuest += OnSetActiveQuestHandler;
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
