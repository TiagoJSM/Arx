using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonInterfaces.Enums;
using UnityEngine;
using GenericComponents.Controllers.Interaction;

[RequireComponent(typeof(SpeechController))]
[RequireComponent(typeof(InteractibleCharacterController))]
[RequireComponent(typeof(CommonTalkingCharacterController))]
public class CommonTalkingCharacterAi : PlatformerCharacterAiControl
{
    private InteractibleCharacterController _interactiveCharacter;
    private CommonTalkingCharacterController _controller;
    private bool _previousIsInteracting;

    protected override Direction CurrentDirection
    {
        get
        {
            return _controller.Direction;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _interactiveCharacter = GetComponent<InteractibleCharacterController>();
        _controller = GetComponent<CommonTalkingCharacterController>();
        _previousIsInteracting = _interactiveCharacter.IsInteracting;
    }

    protected override void Move(float directionValue)
    {
        _controller.Move(directionValue);
    }

    private void Start()
    {
        IddleMovement();
    }

    private void Update()
    {
        if(_previousIsInteracting == _interactiveCharacter.IsInteracting)
        {
            return;
        }

        _previousIsInteracting = _interactiveCharacter.IsInteracting;

        if (_interactiveCharacter.IsInteracting)
        {
            StopActiveCoroutine();
        }
        else
        {
            IddleMovement();
        }
    }
}