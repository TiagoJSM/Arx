using UnityEngine;
using System.Collections;

public interface ITriggerable2D {
    void Triggered2D(GameObject trigger, Collider2D other);
}
