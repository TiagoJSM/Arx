using UnityEngine;
using System.Collections;

public class ContactDetector : MonoBehaviour {

    public int ContactsCount { get; private set; }

    public bool InContact
    {
        get { return ContactsCount != 0; }
    }

    public Collider2D LastContactCollider { get; private set; }

    void OnTriggerEnter2D(Collider2D other) {
        ContactsCount++;
        LastContactCollider = other;
    }

    void OnTriggerExit2D(Collider2D other) {
        ContactsCount--;
        if (ContactsCount == 0)
        {
            LastContactCollider = null;
        }
    }
}
