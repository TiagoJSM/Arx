using UnityEngine;
using System.Collections;
using QuestSystem;
using InventorySystem.Controllers;
using CommonInterfaces.Inventory;
using InventorySystem;

[RequireComponent(typeof(PlatformerCharacterController))]
[RequireComponent(typeof(ItemFinderController))]
[RequireComponent(typeof(InventoryComponent))]
public class PlatformerCharacterUserControl : MonoBehaviour, IQuestSubscriber, IItemOwner
{
    private PlatformerCharacterController _characterController;
    private ItemFinderController _itemFinderController;
    private InventoryComponent _inventoryComponent;
    private bool _jump;

    public event OnKill OnKill;
    public event OnInventoryAdd OnInventoryItemAdd;
    public event OnInventoryRemove OnInventoryItemRemove;

    public InteractionFinderController interactionController;
    
    private void Awake()
    {
        _characterController = GetComponent<PlatformerCharacterController>();
        _itemFinderController = GetComponent<ItemFinderController>();
        _inventoryComponent = GetComponent<InventoryComponent>();
        _itemFinderController.OnInventoryItemFound += OnInventoryItemFoundHandler;
    }
    
    
    private void Update()
    {
        if (!_jump)
        {
            _jump = Input.GetButtonDown("Jump");
        }

        if (interactionController.InteractionTriggerController != null && Input.GetButtonDown("Interact"))
        {
            interactionController.InteractionTriggerController.Interact(this.gameObject);
        }
    }
    
    private void FixedUpdate()
    {
        // Read the inputs.
        //bool crouch = Input.GetKey(KeyCode.LeftControl);
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        // Pass all parameters to the character control script.
        _characterController.Move(horizontal, vertical, _jump);
        _jump = false;
    }

    private void OnInventoryItemFoundHandler(IInventoryItem item)
    {
        _inventoryComponent.Inventory.AddItem(item);
        if (OnInventoryItemAdd != null)
        {
            OnInventoryItemAdd(item);
        }
    }
}
