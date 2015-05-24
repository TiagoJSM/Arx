using UnityEngine;
using System.Collections;

public class InteractionZoneController : MonoBehaviour {

    public InteractionTriggerController triggerController;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerExit2D(Collider2D other)
    {
        triggerController.ColliderExitFromInteractionZone(other);
    }
}
