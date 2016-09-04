using UnityEngine;
using System.Collections;

public class DebugTrigger : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D c)
    {
        Debug.Log("test");
    }
}
