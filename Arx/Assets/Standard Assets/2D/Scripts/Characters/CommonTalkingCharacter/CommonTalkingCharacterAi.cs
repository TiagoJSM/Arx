using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonInterfaces.Enums;
using UnityEngine;
using GenericComponents.Controllers.Interaction;
using Assets.Standard_Assets.UI.Speech.Scripts;

[RequireComponent(typeof(SpeechController))]
[RequireComponent(typeof(InteractibleCharacterController))]
[RequireComponent(typeof(CommonTalkingCharacterController))]
public class CommonTalkingCharacterAi : PlatformerCharacterAiControl
{
    private InteractibleCharacterController _interactiveCharacter;
    private CommonTalkingCharacterController _controller;
    private bool _previousIsInteracting;

    [SerializeField]
    private bool _moveInIddle = true;

    protected override Direction CurrentDirection
    {
        get
        {
            return _controller.Direction;
        }
    }

    protected override Vector2 Velocity
    {
        get
        {
            return _controller.Velocity;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _interactiveCharacter = GetComponent<InteractibleCharacterController>();
        _controller = GetComponent<CommonTalkingCharacterController>();
        _previousIsInteracting = _interactiveCharacter.IsInteracting;
    }

    public override void Move(float directionValue)
    {
        _controller.Move(directionValue);
    }

    private void Start()
    {
        if (_moveInIddle)
        {
            IddleMovement();
        }
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
        else if(_moveInIddle)
        {
            IddleMovement();
        }
    }

}