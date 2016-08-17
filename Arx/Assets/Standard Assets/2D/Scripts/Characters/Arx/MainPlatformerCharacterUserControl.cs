using ArxGame.Components;
using ArxGame.UI;
using CommonInterfaces.Controllers;
using CommonInterfaces.Inventory;
using InventorySystem;
using InventorySystem.Controllers;
using QuestSystem;
using QuestSystem.QuestStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(MainPlatformerController))]
[RequireComponent(typeof(ItemFinderController))]
[RequireComponent(typeof(InventoryComponent))]
[RequireComponent(typeof(QuestLogComponent))]
[RequireComponent(typeof(UiController))]
[RequireComponent(typeof(EquipmentController))]
public class MainPlatformerCharacterUserControl : MonoBehaviour, IQuestSubscriber, IItemOwner, IPlayerControl
{
    private MainPlatformerController _characterController;
    private ItemFinderController _itemFinderController;
    private InventoryComponent _inventoryComponent;
    private QuestLogComponent _questLogComponent;
    private UiController _uiController;
    private EquipmentController _equipmentController;
    private HudManager _hud;
    private bool _jump;

    [SerializeField]
    private InteractionFinder interactionFinder;
    [SerializeField]
    private GameObject HudPrefab;

    public event OnInventoryAdd OnInventoryItemAdd;
    public event OnInventoryRemove OnInventoryItemRemove;
    public event OnKill OnKill;

    public MainPlatformerController PlatformerCharacterController
    {
        get
        {
            return _characterController;
        }
    }

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

    private void Awake()
    {
        _characterController = GetComponent<MainPlatformerController>();
        _itemFinderController = GetComponent<ItemFinderController>();
        _inventoryComponent = GetComponent<InventoryComponent>();
        _questLogComponent = GetComponent<QuestLogComponent>();
        _equipmentController = GetComponent<EquipmentController>();
        _uiController = GetComponent<UiController>();
    }

    private void Start()
    {
        _itemFinderController.OnInventoryItemFound += OnInventoryItemFoundHandler;
        _hud = Instantiate(HudPrefab).GetComponent<HudManager>();
        PlatformerCharacterController.Weapon = _equipmentController.EquippedWeapon;
    }

    private void Update()
    {
        if (!_jump)
        {
            _jump = Input.GetButtonDown("Jump");
        }

        if (Input.GetButtonDown("Interact"))
        {
            var interactionTrigger = interactionFinder.GetInteractionTrigger();
            if(interactionTrigger != null)
            {
                interactionTrigger.Interact(this.gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        //bool crouch = Input.GetKey(KeyCode.LeftControl);
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        _characterController.Move(horizontal, vertical, _jump);

        _jump = false;

        if (Input.GetButtonDown("Fire1"))
        {
            _characterController.LightAttack();
        }
    }

    private void LateUpdate()
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
        if(OnInventoryItemAdd != null)
        {
            OnInventoryItemAdd.Invoke(item);
        }
    }
}

