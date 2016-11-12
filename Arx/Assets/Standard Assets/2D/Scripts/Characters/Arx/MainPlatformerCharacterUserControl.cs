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
using CommonInterfaces.Controllers.Interaction;
using MathHelper;
using MathHelper.Extensions;

[RequireComponent(typeof(MainPlatformerController))]
[RequireComponent(typeof(ItemFinderController))]
[RequireComponent(typeof(InventoryComponent))]
[RequireComponent(typeof(QuestLogComponent))]
[RequireComponent(typeof(UiController))]
[RequireComponent(typeof(EquipmentController))]
public class MainPlatformerCharacterUserControl : MonoBehaviour, IQuestSubscriber, IItemOwner, IPlayerControl
{
    private enum InputAction
    {
        None,
        AttackButtonDown,
        ChargingAttack
    }

    private const float MinAttackChargeTime = 0.2f;

    private InputAction _currentInputAction = InputAction.None;
    private float _attackButtonDownTime;

    private MainPlatformerController _characterController;
    private ItemFinderController _itemFinderController;
    private InventoryComponent _inventoryComponent;
    private QuestLogComponent _questLogComponent;
    private UiController _uiController;
    private EquipmentController _equipmentController;
    private HudManager _hud;
    private bool _jump;
    private Vector3 _interactionPosition;
    private IInteractionTriggerController _currentInteraction;

    [SerializeField]
    private InteractionFinder interactionFinder;
    [SerializeField]
    private GameObject HudPrefab;
    [SerializeField]
    private float _stopInteractionDistance = 3f;
    [SerializeField]
    private GameObject _aimingArm;

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
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        if (vertical > 0)
        {
            var teleporter = FindTeleporter();
            if(teleporter != null)
            {
                teleporter.Teleport(this.gameObject);
            }
        }

        if (!_jump)
        {
            _jump = Input.GetButtonDown("Jump");
        }

        SetAimAngle();
        HandleInteraction();

        _characterController.Move(horizontal, vertical, _jump);

        _jump = false;

        HandleAttack();
        SwitchActiveWeapon();
    }

    private void LateUpdate()
    {
        if (!Input.GetButtonDown("InGameMenu"))
        {
            return;
        }
        _uiController.Toggle();
    }

    private void HandleInteraction()
    {
        var distanceFromInteractionPoint = Vector3.Distance(_interactionPosition, transform.position);
        if (distanceFromInteractionPoint > _stopInteractionDistance && _currentInteraction != null)
        {
            _currentInteraction.StopInteraction();
            _currentInteraction = null;
            return;
        }

        if (!Input.GetButtonDown("Interact"))
        {
            return;
        }
        if (_currentInteraction == null)
        {
            _currentInteraction = interactionFinder.GetInteractionTrigger();
            _interactionPosition = transform.position;
        }

        if (_currentInteraction != null)
        {
            _currentInteraction.Interact(this.gameObject);
        }
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

    private ITeleporter FindTeleporter()
    {
        var collider = 
            Physics2D
                .OverlapPointAll(this.transform.position)
                .FirstOrDefault(c => c.GetComponent<ITeleporter>() != null);

        if(collider == null)
        {
            return null;
        }

        return collider.GetComponent<ITeleporter>();
    }

    private void HandleAttack()
    {
        switch (_currentInputAction)
        {
            case InputAction.None: NoneAttackState(); break;
            case InputAction.AttackButtonDown: AttackButtonDownState(); break;
            case InputAction.ChargingAttack: ChargingAttackState(); break;
        }
    }

    private void SwitchActiveWeapon()
    {
        if (_characterController.Attacking)
        {
            return;
        }
        if (Input.GetButtonDown("SetWeaponSocket1"))
        {
            _equipmentController.ActiveWeaponSocket = WeaponSocket.Weapon1;
            PlatformerCharacterController.Weapon = _equipmentController.EquippedWeapon;
        }
        else if (Input.GetButtonDown("SetWeaponSocket2"))
        {
            _equipmentController.ActiveWeaponSocket = WeaponSocket.Weapon2;
            PlatformerCharacterController.Weapon = _equipmentController.EquippedWeapon;
        }
        else if (Input.GetButtonDown("SetWeaponSocket3"))
        {
            _equipmentController.ActiveWeaponSocket = WeaponSocket.Weapon3;
            PlatformerCharacterController.Weapon = _equipmentController.EquippedWeapon;
        }
        else if (Input.GetButtonDown("SetWeaponSocket4"))
        {
            _equipmentController.ActiveWeaponSocket = WeaponSocket.Weapon4;
            PlatformerCharacterController.Weapon = _equipmentController.EquippedWeapon;
        }
    }

    private void NoneAttackState()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _attackButtonDownTime = 0;
            _currentInputAction = InputAction.AttackButtonDown;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            _characterController.StrongAttack();
        }
    }

    private void AttackButtonDownState()
    {
        _attackButtonDownTime += Time.deltaTime;
        if(_attackButtonDownTime > MinAttackChargeTime)
        {
            _characterController.ChargeAttack();
            _currentInputAction = InputAction.ChargingAttack;
            return;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            _characterController.LightAttack();
            _currentInputAction = InputAction.None;
        }
    }

    private void ChargingAttackState()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            _characterController.ReleaseChargeAttack();
            _currentInputAction = InputAction.None;
        }
    }

    private void SetAimAngle()
    {
        var center = _aimingArm.transform.position;
        var aimPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var degrees = FloatUtils.AngleBetween(center, aimPosition).ReduceToSingleTurn();
        _characterController.AimAngle = degrees;
    }
}

