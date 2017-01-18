using ArxGame.UI;
using Assets.Standard_Assets.QuestSystem.QuestStructures;
using Assets.Standard_Assets.UI.Quest_Section.Scripts;
using InventorySystem;
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

        public InventorySectionManager inventorySection;
        public QuestSection questSection;

        public InventoryComponent inventoryComponent;
        public QuestLogComponent questLogComponent;

        public event OnQuestSelected OnSetActiveQuest
        {
            add
            {
                questSection.OnSetActiveQuest += value;
            }
            remove
            {
                questSection.OnSetActiveQuest -= value;
            }
        }

        public void SetInventorySection(bool toggle)
        {
            if (toggle)
            {
                SetSection(inventorySection.gameObject);
                _currentSection.GetComponent<InventorySectionManager>().Initialize(inventoryComponent);
            }
        }

        public void SetQuestSection(bool toggle)
        {
            if (toggle)
            {
                SetSection(questSection.gameObject);
                _currentSection.GetComponent<QuestSection>().Initialize(questLogComponent);
            }
        }

        private void SetSection(GameObject section)
        {
            if (_currentSection != null)
            {
                Destroy(_currentSection);
            }

            _currentSection = Instantiate(section);
            _currentSection.transform.SetParent(this.transform, false);
        }
    }
}
