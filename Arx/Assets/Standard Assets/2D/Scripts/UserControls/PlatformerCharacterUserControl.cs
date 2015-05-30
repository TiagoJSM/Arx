using UnityEngine;
using System.Collections;
using QuestSystem;

[RequireComponent(typeof (PlatformerCharacterController))]
public class PlatformerCharacterUserControl : MonoBehaviour, IQuestSubscriber
{
    private PlatformerCharacterController _characterController;
    private bool _jump;

    public event OnKill OnKill;
    public event OnInventoryAdd OnInventoryItemAdd;
    public event OnInventoryRemove OnInventoryItemRemove;

    public InteractionFinderController interactionController;
    
    private void Awake()
    {
        _characterController = GetComponent<PlatformerCharacterController>();
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
}
