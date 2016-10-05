using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonInterfaces.Enums;
using UnityEngine;
using GenericComponents.Controllers.Interaction;

[RequireComponent(typeof(SpeechController))]
[RequireComponent(typeof(CommonTalkingCharacterController))]
public class CommonTalkingCharacterAi : PlatformerCharacterAiControl
{
    private SpeechController _speechController;
    private CommonTalkingCharacterController _controller;

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
        _speechController = GetComponent<SpeechController>();
        _controller = GetComponent<CommonTalkingCharacterController>();

        _speechController.OnVisibilityChange += OnVisibilityChangeHandler;
    }

    protected override void Move(float directionValue)
    {
        _controller.Move(directionValue);
    }

    private void Start()
    {
        IddleMovement();
    }

    private void OnDestroy()
    {
        _speechController.OnVisibilityChange -= OnVisibilityChangeHandler;
    }

    private void OnVisibilityChangeHandler(bool visible)
    {
        if (visible)
        {
            StopActiveCoroutine();
        }
        else
        {
            IddleMovement();
        }
    }
}