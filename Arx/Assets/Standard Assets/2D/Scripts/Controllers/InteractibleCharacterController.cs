using UnityEngine;
using System.Collections;
using CommonInterfaces.Controllers.Interaction;
using GenericComponents.Controllers.Interaction;

public class InteractibleCharacterController : MonoBehaviour {

    private IInteractionTriggerController _interactionController;
    private SpeechController _speechController;

	void Start () 
    {
        _interactionController = GetComponentInChildren<IInteractionTriggerController>();
        _speechController = GetComponentInChildren<SpeechController>();
        _interactionController.OnInteract += OnInteractHandler;
        _interactionController.OnStopInteraction += OnStopInteractionHandler;
        _speechController.OnScrollEnd += OnScrollEndHandler;

    }

    private void OnInteractHandler(GameObject interactor)
    {
        if (!_speechController.Visible)
        {
            _speechController.Reset();
            _speechController.Visible = true;
            return;
        }
        _speechController.ScrollPageDown();
    }

    private void OnStopInteractionHandler()
    {
        _speechController.Visible = false;
    }

    private void OnScrollEndHandler()
    {
        _speechController.Visible = false;
    }
}
