using CommonInterfaces.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(MainPlatformerController))]
public class MainPlatformerControllerCutsceneController : MonoBehaviour
{
    private MainPlatformerController _platformerController;

    void Awake()
    {
        _platformerController = GetComponent<MainPlatformerController>();
    }

    public void MoveInDirection(Direction direction)
    {
        _platformerController.Move(direction == Direction.Left ? -1 : 1, 0, null, false, false, false);
    }
}
