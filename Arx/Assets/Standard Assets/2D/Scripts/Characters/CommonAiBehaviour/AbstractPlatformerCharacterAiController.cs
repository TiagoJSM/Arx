using Assets.Standard_Assets._2D.Scripts.Characters.CommonAiBehaviour;
using CommonInterfaces.Enums;
using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class AbstractPlatformerCharacterAiController : BaseCharacterAiController
{
    private Vector3 _startingPosition;

    [SerializeField]
    private float _targetRange = 1;
    [SerializeField]
    private float _maxDistanceFromStartingPoint = 10;
    [SerializeField]
    private float _maxStoppedIddleTime = 5;
    [SerializeField]
    private Transform _fallDetector;
    [SerializeField]
    private float _fallHeight = 7;
    [SerializeField]
    private LayerMask _groundMask = 0;

    protected abstract Direction CurrentDirection{ get; }
    protected abstract Vector2 Velocity { get; }
    public GameObject Target { get; protected set; }

    public bool IsTargetInRange
    {
        get
        {
            var currentPosition = this.transform.position;
            var distance = Vector2.Distance(currentPosition, Target.transform.position);
            return distance < _targetRange;
        }
    }

    protected virtual void Awake()
    {
        _startingPosition = this.transform.position;
    }

    public abstract void Move(float directionValue);

    protected void IddleMovement()
    {
        SetActiveCoroutine(IddleMovementCoroutine());
    }

    public bool CanMoveToGroundAhead()
    {
        if(_fallDetector == null)
        {
            return true;
        }
        return Physics2D.Raycast(_fallDetector.position, Vector2.down, _fallHeight, _groundMask);
    }

    private IEnumerator IddleMovementCoroutine()
    {
        var currentDirectionMovement = CurrentDirection;

        while (true)
        {
            Move(currentDirectionMovement.DirectionValue());
            yield return null;
            var distance = Vector2.Distance(_startingPosition, this.transform.position);
            var directionOfStartingPoint = (this.transform.position.x - _startingPosition.x) >= 0 ? Direction.Right : Direction.Left;
            if (WaitForDirectionChange(distance, directionOfStartingPoint, currentDirectionMovement))
            {
                currentDirectionMovement = currentDirectionMovement == Direction.Left ? Direction.Right : Direction.Left;
                var stopTime = UnityEngine.Random.Range(0, _maxStoppedIddleTime);
                yield return new WaitForSeconds(stopTime);
            }
        }
    }

    private bool WaitForDirectionChange(
        float distance, 
        Direction directionOfStartingPoint, 
        Direction currentDirectionMovement)
    {
        var arrivedToDestination = distance >= _maxDistanceFromStartingPoint && directionOfStartingPoint == currentDirectionMovement;

        if (arrivedToDestination || !CanMoveToGroundAhead())
        {
            return true;
        }

        return Mathf.Approximately(Velocity.x, 0);
    }

    protected virtual void OnDrawGizmos()
    {
        if(_fallDetector != null)
        {
            Gizmos.color = Color.green;
            var direction = Vector2.down * _fallHeight;
            Gizmos.DrawRay(_fallDetector.position, direction);
        }
    }
}