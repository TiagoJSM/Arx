using UnityEngine;
using System.Collections;

public class LedgeCheck : MonoBehaviour {

    private ILedgeGrabber ledgeGrabber;

    public ContactDetector ledgeWallDetector;
    public ContactDetector emptySpaceDetector;

    public bool LedgeDetected
    {
        get
        {
            return ledgeWallDetector.InContact && !emptySpaceDetector.InContact;
        }
    }

    public Collider2D LastContactCollider 
    {
        get
        {
            return ledgeWallDetector.LastContactCollider;
        }
    }

    void Start()
    {
        ledgeGrabber = GetComponentInParent<ILedgeGrabber>();
    }

    void FixedUpdate () {
        if (ledgeGrabber == null)
        {
            return;
        }

        ledgeGrabber.LedgeDetected(LedgeDetected, LastContactCollider);
	}
}
