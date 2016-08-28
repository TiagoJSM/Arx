using UnityEngine;
using System.Collections;

public class ContactDetector : MonoBehaviour {

    public bool InContact
    {
        get { return LastContactCollider != null; }
    }

    public Collider2D LastContactCollider { get; private set; }

    void OnTriggerEnter2D(Collider2D other) {
        LastContactCollider = other;
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other == LastContactCollider)
        {
            LastContactCollider = null;
        }
    }
}
