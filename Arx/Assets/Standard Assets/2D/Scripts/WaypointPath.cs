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
            if(pair.Item1 == null || pair.Item2 == null)
            {
                continue;
            }
            Gizmos.DrawLine(pair.Item1.transform.position, pair.Item2.transform.position);
        }
    }
}
