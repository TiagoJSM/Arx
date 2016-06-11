using ArxGame.UI;
using CommonInterfaces.Controllers;
using CommonInterfaces.Inventory;
using GenericComponents.UserControls;
using InventorySystem;
using InventorySystem.Controllers;
using QuestSystem;
using QuestSystem.QuestStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components
{
    [RequireComponent(typeof(ItemFinderController))]
    [RequireComponent(typeof(InventoryComponent))]
    [RequireComponent(typeof(QuestLogComponent))]
    [RequireComponent(typeof(UiController))]
    public class MainPlatformerCharacterUserControl : PlatformerCharacterUserControl, IQuestSubscriber, IItemOwner, IPlayerControl
    {
        private ItemFinderController _itemFinderController;
        private InventoryComponent _inventoryComponent;
        private QuestLogComponent _questLogComponent;
        private UiController _uiController;
        private HudManager _hud;

        public GameObject HudPrefab;

        public event OnInventoryAdd OnInventoryItemAdd;
        public event OnInventoryRemove OnInventoryItemRemove;
        public event OnKill OnKill;

        public void AssignQuest(Quest quest)
        {
            throw new NotImplementedException();
        }

        public Quest GetQuest(string name)
        {
            throw new NotImplementedException();
        }

        public bool HasQuest(Quest quest)
        {
            throw new NotImplementedException();
        }

        void Start()
        {
            _itemFinderController = GetComponent<ItemFinderController>();
            _inventoryComponent = GetComponent<InventoryComponent>();
            _questLogComponent = GetComponent<QuestLogComponent>();
            _uiController = GetComponent<UiController>();

            _itemFinderController.OnInventoryItemFound += OnInventoryItemFoundHandler;
            _hud = Instantiate(HudPrefab).GetComponent<HudManager>();
        }

        void LateUpdate()
        {
            if (!Input.GetButtonDown("InGameMenu"))
            {
                return;
            }
            _uiController.Toggle();
        }

        private void OnInventoryItemFoundHandler(IInventoryItem item)
        {
            _inventoryComponent.Inventory.AddItem(item);
            _hud.Toast("Item found: " + item.Name, _hud.Short);
            if (OnInventoryItemAdd != null)
            {
                OnInventoryItemAdd(item);
            }
        }
    }
}
