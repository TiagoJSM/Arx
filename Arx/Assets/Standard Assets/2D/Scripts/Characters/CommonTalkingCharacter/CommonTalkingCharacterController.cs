using GenericComponents.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CommonTalkingCharacterController : PlatformerCharacterController
{
    private float _move;

    public void Move(float move)
    {
        _move = move;
    }

    protected override void Update()
    {
        base.Update();
        if(_move != 0)
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
