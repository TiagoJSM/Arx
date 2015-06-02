using UnityEngine;
using System.Collections;
using QuestSystem;
using InventorySystem.Controllers;
using CommonInterfaces.Inventory;

[RequireComponent(typeof(PlatformerCharacterController))]
[RequireComponent(typeof(ItemFinderController))]
public class PlatformerCharacterUserControl : MonoBehaviour, IQuestSubscriber
{
    private PlatformerCharacterController _characterController;
    private ItemFinderController _itemFinderController;
    private bool _jump;

    public event OnKill OnKill;
    public event OnInventoryAdd OnInventoryItemAdd;
    public event OnInventoryRemove OnInventoryItemRemove;

    public InteractionFinderController interactionController;
    
    private void Awake()
    {
        _characterController = GetComponent<PlatformerCharacterController>();
        _itemFinderController = GetComponent<ItemFinderController>();
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

    }
}
