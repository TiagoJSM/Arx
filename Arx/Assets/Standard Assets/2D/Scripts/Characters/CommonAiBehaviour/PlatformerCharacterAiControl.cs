using CommonInterfaces.Enums;
using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class PlatformerCharacterAiControl : MonoBehaviour
{
    private Vector3 _startingPosition;
    private Coroutine _activeCoroutine;

    [SerializeField]
    private float _maxDistanceFromStartingPoint = 10;
    [SerializeField]
    private float _maxStoppedIddleTime = 5;

    protected abstract Direction CurrentDirection{ get; }

    protected virtual void Awake()
    {
        _startingPosition = this.transform.position;
    }

    protected abstract void Move(float directionValue);

    protected void IddleMovement()
    {
        SetActiveCoroutine(IddleMovementCoroutine());
    }

    protected void SetActiveCoroutine(IEnumerator coroutine)
    {
        StopActiveCoroutine();
        _activeCoroutine = StartCoroutine(coroutine);
    }

    protected void StopActiveCoroutine()
    {
        if (_activeCoroutine != null)
        {
            StopCoroutine(_activeCoroutine);
        }
        _activeCoroutine = null;
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
            if (distance >= _maxDistanceFromStartingPoint && directionOfStartingPoint == currentDirectionMovement)
            {
                currentDirectionMovement = currentDirectionMovement == Direction.Left ? Direction.Right : Direction.Left;
                var stopTime = UnityEngine.Random.Range(0, _maxStoppedIddleTime);
                yield return new WaitForSeconds(stopTime);
            }
        }
    }
}