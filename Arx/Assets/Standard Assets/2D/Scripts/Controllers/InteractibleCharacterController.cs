using UnityEngine;
using System.Collections;
using CommonInterfaces.Controllers.Interaction;
using GenericComponents.Controllers.Interaction;
using CommonInterfaces;
using System;

public class InteractibleCharacterController : MonoBehaviour, IInteractionTriggerController
{
    private SpeechController _speechController;
    private int _dialogIdx;

    [Speakers]
    [SerializeField]
    private InteractiveDialog _dialog;

    public event OnInteract OnInteract;
    public event OnStopInteraction OnStopInteraction;

    public bool IsInteracting
    {
        get
        {
            return _speechController != null;
        }
    }

    public void Interact(GameObject interactor)
    {
        if(_speechController == null)
        {
            Speak();
            return;
        }
        _speechController.Continue();
    }

    public void StopInteraction()
    {
        if(_speechController != null)
        {
            _speechController.Close();
            _speechController.OnScrollEnd -= OnScrollEndHandler;
            _speechController = null;
        }
        _dialogIdx = 0;
    }

    private void Speak()
    {
        var context = _dialog.GetDialogContext(_dialogIdx);
        _speechController = context.First;
        _speechController.OnScrollEnd += OnScrollEndHandler;
        _speechController.Say(context.Second);
    }

    private void OnScrollEndHandler()
    {
        _speechController.Close();
        _speechController.OnScrollEnd -= OnScrollEndHandler;
        _speechController = null;
        _dialogIdx++;
        if(_dialogIdx >= _dialog.DialogsCount)
        {
            _dialogIdx = 0;
            return;
        }
        Speak();
    }
}
