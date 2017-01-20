using UnityEngine;
using System.Collections;
using CommonInterfaces.Controllers.Interaction;
using GenericComponents.Controllers.Interaction;
using CommonInterfaces;
using System;
using Assets.Standard_Assets._2D.Scripts.Controllers;

public class InteractibleCharacterController : MonoBehaviour, IInteractionTriggerController
{
    private SpeechController _speechController;
    private int _dialogIdx;
    private GameObject _interactor;

    [SerializeField]
    private InteractiveDialogComponent _dialog;

    public event OnInteract OnInteract;
    public event OnStopInteraction OnStopInteraction;
    public event OnInteract OnInteractionComplete;

    public bool IsInteracting
    {
        get
        {
            return _speechController != null;
        }
    }

    public InteractiveDialogComponent Dialog
    {
        get { return _dialog; }
        set { _dialog = value; }
    }

    public void Interact(GameObject interactor)
    {
        _interactor = interactor;
        if(OnInteract != null)
        {
            OnInteract(interactor);
        }
        if (_speechController == null)
        {
            Speak(interactor);
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

    private void Speak(GameObject interactor)
    {
        var context = _dialog.GetDialogContext(_dialogIdx);
        _speechController =
            context.interacterSpeaking
            ? interactor.GetComponent<SpeechController>()
            : context.speechController;

        _speechController.OnScrollEnd += OnScrollEndHandler;
        _speechController.Say(context.text);
        
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
            if(OnInteractionComplete != null)
            {
                OnInteractionComplete(_interactor);
            }
            return;
        }
        Speak(_interactor);
    }
}
