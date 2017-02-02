using Assets.Standard_Assets.InventorySystem;
using Assets.Standard_Assets.QuestSystem.QuestStructures;
using Assets.Standard_Assets.UI.In_Game_Menu.Scripts;
using Assets.Standard_Assets.UI.Quest_Section.Scripts;
using CommonInterfaces.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx
{
    [RequireComponent(typeof(InventoryComponent))]
    [RequireComponent(typeof(QuestLogComponent))]
    public class UiController : MonoBehaviour, IUiController
    {
        private UiMenuManager _inGameMenu;
        private InventoryComponent _inventoryComponent;
        private QuestLogComponent _questLogComponent;

        public UiMenuManager inGameMenuPrefab;

        public event OnQuestSelected OnActiveQuestSelected;

        public bool IsMenuOn
        {
            get
            {
                return _inGameMenu != null;
            }
        }

        public void TurnOn()
        {
            if (IsMenuOn)
            {
                return;
            }
            _inGameMenu = Instantiate(inGameMenuPrefab);
            _inGameMenu.inventoryComponent = _inventoryComponent;
            _inGameMenu.questLogComponent = _questLogComponent;
            _inGameMenu.OnSetActiveQuest += OnSetActiveQuestHandler;
        }

        public void TurnOff()
        {
            if (!IsMenuOn)
            {
                return;
            }
            _inGameMenu.OnSetActiveQuest -= OnSetActiveQuestHandler;
            Destroy(_inGameMenu.gameObject);
            _inGameMenu = null;
        }

        public void Toggle()
        {
            if (IsMenuOn)
            {
                TurnOff();
            }
            else
            {
                TurnOn();
            }
        }

        void Start()
        {
            _inventoryComponent = GetComponent<InventoryComponent>();
            _questLogComponent = GetComponent<QuestLogComponent>();
        }

        private void OnSetActiveQuestHandler(Quest quest)
        {
            if(OnActiveQuestSelected != null)
            {
                OnActiveQuestSelected(quest);
            }
        }
    }
}
