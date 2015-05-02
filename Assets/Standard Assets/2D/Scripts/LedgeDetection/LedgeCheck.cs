using UnityEngine;
using System.Collections;

public class LedgeCheck : MonoBehaviour {

    public ContactDetector ledgeWallDetector;
    public ContactDetector emptySpaceDetector;
    public MonoBehaviour ledgeGrabber;
	
	void FixedUpdate () {
        var lGrabber = ledgeGrabber as ILedgeGrabber;
        if (lGrabber == null)
        {
            return;
        }

        var ledgeDetected = ledgeWallDetector.InContact && !emptySpaceDetector.InContact;
        lGrabber.LedgeDetected(ledgeDetected, ledgeWallDetector.LastContactCollider);
	}
}
