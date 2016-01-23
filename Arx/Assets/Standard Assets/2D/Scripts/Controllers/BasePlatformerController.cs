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

    protected void Flip(bool right)
    {
        var scale = transform.localScale;
        if ((right && scale.x > 0) || (!right && scale.x < 0))
        {
            return;
        }
        scale.x *= -1;
        transform.localScale = scale;
    }

    protected bool DirectionOfMovement(float horizontal, bool defaultValue)
    {
        if (horizontal > 0)
        {
            return true;
        }
        else if (horizontal < 0)
        {
            return false;
        }
        return defaultValue;
    }
}
