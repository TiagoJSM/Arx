using UnityEngine;
using System.Collections;

public class Camera2DAhead : MonoBehaviour
{
    public Transform target;
    public float damping = 0.1f;
    public float lookAheadFactor = 3;
    public float lookAheadMoveThreshold = 0.03f;
    
    private float _offsetZ;
    private Vector3 _lastTargetPosition;
    private float _currentVelocity;
    private Vector3 _lookAheadPos;
    
    // Use this for initialization
    private void Start()
    {
        _lastTargetPosition = target.position;
        _offsetZ = (transform.position - target.position).z;
        transform.parent = null;
    }

    // Update is called once per frame
    private void Update()
    {
        // only update lookahead pos if accelerating or changed direction
        float xMoveDelta = (target.position - _lastTargetPosition).x;
        
        bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

        if (updateLookAheadTarget)
        {
            _lookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
        }
        
        Vector3 aheadTargetPos = target.position + _lookAheadPos + Vector3.forward*_offsetZ;
        aheadTargetPos.x = Mathf.SmoothDamp(transform.position.x, aheadTargetPos.x, ref _currentVelocity, damping);

        transform.position = aheadTargetPos;
        
        _lastTargetPosition = target.position;
    }
}
