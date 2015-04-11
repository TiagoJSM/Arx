using UnityEngine;
using System.Collections;

public class WaypointPathController : MonoBehaviour {

    private int _pathIdx;
    private Rigidbody2D _rigidbody;
    private GameObject _instantiatedPlatform;

    public float waypointThreasholdRadius = 0.2f;
    public WaypointPath path;
    public GameObject platform;

	// Use this for initialization
	void Start () {
        var instatiatePosition = GetWaypoint().gameObject.transform.position;
        _instantiatedPlatform = Instantiate(platform, instatiatePosition, Quaternion.identity) as GameObject;
        _rigidbody = _instantiatedPlatform.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateWaypointIndex();
        var normalized = (GetWaypoint().gameObject.transform.position - _instantiatedPlatform.transform.position).normalized;
        _rigidbody.velocity = normalized.ToVector2();
	}

    private Waypoint GetWaypoint()
    {
        return path.waypoints [_pathIdx];
    }

    private void UpdateWaypointIndex()
    {
        var waypoint = GetWaypoint();
        var distance = 
            Vector3
                .Distance(
                    _instantiatedPlatform.transform.position, 
                    waypoint.gameObject.transform.position);

        if (distance <= waypointThreasholdRadius)
        {
            _pathIdx++;
        }

        if (_pathIdx >= path.waypoints.Length)
        {
            _pathIdx = 0;
        }
    }
}
