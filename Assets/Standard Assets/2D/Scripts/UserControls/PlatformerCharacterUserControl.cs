using UnityEngine;
using System.Collections;

[RequireComponent(typeof (PlatformerCharacterController))]
public class PlatformerCharacterUserControl : MonoBehaviour
{
    private PlatformerCharacterController _controller;
    private bool _jump;
    
    
    private void Awake()
    {
        _controller = GetComponent<PlatformerCharacterController>();
    }
    
    
    private void Update()
    {
        if (!_jump)
        {
            _jump = Input.GetButtonDown("Jump");
        }
    }
    
    
    private void FixedUpdate()
    {
        // Read the inputs.
        //bool crouch = Input.GetKey(KeyCode.LeftControl);
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        // Pass all parameters to the character control script.
        _controller.Move(horizontal, vertical, _jump);
        _jump = false;
    }
}
