using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos () 
    {
        DrawIcon();
    }

    void OnDrawGizmosSelected () 
    {
        DrawIcon();
    }

    private void DrawIcon()
    {
        Gizmos.DrawIcon (transform.position, "waypoint.png");
    }
}
