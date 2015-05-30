using UnityEngine;
using System.Collections;

public class LedgeCheck : MonoBehaviour {

    public ContactDetector ledgeWallDetector;
    public ContactDetector emptySpaceDetector;
    public MonoBehaviour ledgeGrabber;

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
	
	void FixedUpdate () {
        var lGrabber = ledgeGrabber as ILedgeGrabber;
        if (lGrabber == null)
        {
            return;
        }

        lGrabber.LedgeDetected(LedgeDetected, LastContactCollider);
	}
}
