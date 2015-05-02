using UnityEngine;
using System;

public interface ILedgeGrabber
{
    void LedgeDetected(bool detected, Collider2D ledgeCollider);
}


