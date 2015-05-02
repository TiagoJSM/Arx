using UnityEngine;
using System.Collections;

public class BasePlatformerController : MonoBehaviour {

    private Collider2D activePlatformCollider; 

    void OnCollisionEnter2D(Collision2D other) 
    {
        if (activePlatformCollider != null)
        {
            return;
        }
        foreach (var contact in other.contacts)
        {
            if (contact.normal.y > 0.5) 
            { 
                transform.parent = other.transform;
                activePlatformCollider = contact.collider;
                return;
            }
        }
        activePlatformCollider = null;
    }
    
    void OnCollisionExit2D(Collision2D other) 
    {
        if (other.collider == activePlatformCollider)
        {
            transform.parent = null;
            activePlatformCollider = null;
        }
    }
}
