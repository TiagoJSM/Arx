using UnityEngine;
using System.Collections;

public class WaypointPathController : MonoBehaviour {

    private int _pathIdx;
    private Rigidbody2D _rigidbody;
    private Transform _platformTransform;

    public float waypointThreasholdRadius = 0.2f;
    public WaypointPath path;
    public GameObject platform;
    public float velocity = 1;

	// Use this for initialization
	void Start () {
        _rigidbody = platform.GetComponent<Rigidbody2D>();
        _platformTransform = platform.transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        UpdateWaypointIndex();
        var normalized = (GetWaypoint().gameObject.transform.position.ToVector2() - _rigidbody.position).normalized;
        var position = _rigidbody.position + normalized * velocity * Time.deltaTime;
        _rigidbody.MovePosition(position);
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
                    _platformTransform.position, 
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
