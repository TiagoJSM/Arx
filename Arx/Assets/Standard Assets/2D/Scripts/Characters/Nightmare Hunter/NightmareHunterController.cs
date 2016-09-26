using UnityEngine;
using System.Collections;
using GenericComponents.Controllers.Characters;

public class NightmareHunterController : PlatformerCharacterController
{
    private float _move;

    public void Move(float move)
    {
        _move = move;
    }

    protected override void Update()
    {
        base.Update();
        if (_move != 0)
        {
            DoMove(_move);
        }
        else
        {
            StayStill();
        }
        _move = 0;
    }
}
