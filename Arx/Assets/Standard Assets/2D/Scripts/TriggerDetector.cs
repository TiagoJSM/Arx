using UnityEngine;
using System.Collections;

public class TriggerDetector : MonoBehaviour {

    public Object triggerable2D;

    void OnTriggerEnter2D(Collider2D other) {
        ITriggerable2D trig = triggerable2D as ITriggerable2D;
        if (trig == null)
        {
            return;
        }
        trig.Triggered2D(gameObject, other);
    }
}
