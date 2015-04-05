using UnityEngine;
using System;

public interface ILedgeGrabber
{
    void CanGrabLedge(bool canGrab, Collider2D ledgeCollider);
}


