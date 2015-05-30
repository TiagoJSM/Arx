using UnityEngine;
using System.Collections;

public class InteractionTriggerController : MonoBehaviour {

    private GameObject _interactor;

	public Canvas canvas;

	// Use this for initialization
	void Start () {
        canvas.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ColliderExitFromInteractionZone(Collider2D other)
    {
        if (other.gameObject == _interactor)
        {
            _interactor = null;
            StopInteraction();
        }
    }

    public void Interact(GameObject interactor)
    {
        _interactor = interactor;
        canvas.enabled = true;
    }

    public void StopInteraction()
    {
        canvas.enabled = false;
    }
}
