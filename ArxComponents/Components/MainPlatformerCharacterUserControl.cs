using ArxGame.Components.UserControls;
using ArxGame.Components.Weapons;
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
    [RequireComponent(typeof(CharacterStatus))]
    [RequireComponent(typeof(EquipmentController))]
    public class MainPlatformerCharacterUserControl : PlatformerCharacterUserControl, IQuestSubscriber, IItemOwner, IPlayerControl, ICharacter
    {
        private ItemFinderController _itemFinderController;
        private InventoryComponent _inventoryComponent;
        private QuestLogComponent _questLogComponent;
        private UiController _uiController;
        private CharacterStatus _characterStatus;
        private EquipmentController _equipmentController;
        private HudManager _hud;

        public GameObject HudPrefab;

        public bool CanBeAttacked
        {
            get
            {
                return true;
            }
        }

        public bool IsEnemy
        {
            get
            {
                return false;
            }
        }

        public int MaxLifePoints
        {
            get
            {
                return _characterStatus.health.maxLifePoints;
            }
        }

        public int LifePoints
        {
            get
            {
                return _characterStatus.health.lifePoints;
            }
        }

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
            _characterStatus = GetComponent<CharacterStatus>();
            _equipmentController = GetComponent<EquipmentController>();
            _uiController = GetComponent<UiController>();

            _itemFinderController.OnInventoryItemFound += OnInventoryItemFoundHandler;
            _hud = Instantiate(HudPrefab).GetComponent<HudManager>();
            PlatformerCharacterController.Weapon = _equipmentController.EquippedWeapon;
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
            OnInventoryItemAdd?.Invoke(item);
        }

        public bool Attacked(GameObject attacker, int damage, Vector3? hitPoint)
        {
            return false;
        }
    }
}
