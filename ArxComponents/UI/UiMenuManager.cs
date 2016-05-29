using InventorySystem;
using QuestSystem.QuestStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ArxGame.UI
{
    public class UiMenuManager : MonoBehaviour
    {
        private GameObject _currentSection;

        public GameObject inventorySection;
        public GameObject questSection;

        public InventoryComponent inventoryComponent;
        public QuestLogComponent questLogComponent;

        public void SetInventorySection(bool toggle)
        {
            if (toggle)
            {
                SetSection(inventorySection);
                inventorySection.GetComponent<InventorySectionManager>().InventoryComponent = inventoryComponent;
            }
        }

        public void SetQuestSection(bool toggle)
        {
            if (toggle)
            {
                SetSection(questSection);
            }
        }

        private void SetSection(GameObject section)
        {
            if(_currentSection != null)
            {
                Destroy(_currentSection);
            }

            _currentSection = Instantiate(section);
            _currentSection.transform.SetParent(this.transform, false);
        }
    }
}
