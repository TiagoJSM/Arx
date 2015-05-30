using UnityEngine;
using System.Collections;

public class WaypointPath : MonoBehaviour {

    public Waypoint[] waypoints;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos () 
    {
        DrawPath();
    }
    
    void OnDrawGizmosSelected () 
    {
        DrawPath();
    }
    
    private void DrawPath()
    {
        foreach (var pair in waypoints.ToPairs())
        {
            Gizmos.color = Color.blue;
            if(pair.First == null || pair.Second == null)
            {
                continue;
            }
            Gizmos.DrawLine(pair.First.transform.position, pair.Second.transform.position);
        }
    }
}
