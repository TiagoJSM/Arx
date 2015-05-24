using UnityEngine;
using System.Collections;

public class InteractionFinderController : MonoBehaviour {

    public InteractionTriggerController InteractionTriggerController { get; private set; }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        var controller = GetInteractionTriggerController(other);
        if (controller == null)
        {
            return;
        }
        InteractionTriggerController = controller;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var controller = GetInteractionTriggerController(other);
        if (controller == null)
        {
            return;
        }
        if (InteractionTriggerController == controller)
        {
            InteractionTriggerController = null;
        }
    }

    private InteractionTriggerController GetInteractionTriggerController(Collider2D other)
    {
        if (other.gameObject.tag != Tags.InteractionRadiusObject)
        {
            return null;
        }
        var parent = other.gameObject.transform.parent;
        if (parent == null)
        {
            return null;
        }
        var controller = parent.GetComponent<InteractionTriggerController>();
        if (controller == null)
        {
            return null;
        }
        return controller;
    }
}
