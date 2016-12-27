using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;

public class LedgeChecker : MonoBehaviour
{
    private Collider2D[] _resultBuffer = new Collider2D[10];

    private Collider2D _ledge;

    [SerializeField]
    private Transform _detectionBoxP1;
    [SerializeField]
    private Transform _detectionBoxP2;
    [SerializeField]
    [Range(0, 1)]
    private float _detectionBoxVerticalSplit = 0.5f;
    [SerializeField]
    [Range(0, 1)]
    private float _ledgeDetectorWidthPercentage = 0.5f;
    [SerializeField]
    private LayerMask _whatIsGround;

    public float LedgeDetectorWidth
    {
        get
        {
            var width = _detectionBoxP2.position.x - _detectionBoxP1.position.x;
            return width * _ledgeDetectorWidthPercentage;
        }
    }

    public float LedgeDetectorHeight
    {
        get
        {
            var height = _detectionBoxP2.position.y - _detectionBoxP1.position.y;
            return height * _detectionBoxVerticalSplit;
        }
    }

    public Vector2 LedgeDetectorUpperBound
    {
        get
        {
            var height = _detectionBoxP2.position.y - _detectionBoxP1.position.y;
            return new Vector2(
                _detectionBoxP1.position.x + LedgeDetectorWidth,
                _detectionBoxP1.position.y + LedgeDetectorHeight);
        }
    }

    public float FreeSpaceWidth
    {
        get
        {
            return _detectionBoxP2.position.x - _detectionBoxP1.position.x;
        }
    }

    public float FreeSpaceHeight
    {
        get
        {
            var height = _detectionBoxP2.position.y - _detectionBoxP1.position.y;
            return height * (1 - _detectionBoxVerticalSplit);
        }
    }

    public Vector2 FreeSpaceLowerBound
    {
        get
        {
            return new Vector2(_detectionBoxP1.position.x, _detectionBoxP1.position.y + LedgeDetectorHeight);
        }
    }

    public bool IsLedgeDetected(out Collider2D ledge)
    {
        if (_detectionBoxP2 == null || _detectionBoxP1 == null)
        {
            ledge = null;
            return false;
        }
        var ledgeDetectorUpperBound = LedgeDetectorUpperBound;
        var freeSpaceLowerBound = FreeSpaceLowerBound;

        var count = Physics2D.OverlapAreaNonAlloc(_detectionBoxP1.transform.position, ledgeDetectorUpperBound, _resultBuffer, _whatIsGround);
        ledge = null;
        for(var idx = 0; idx < count; idx++)
        {
            var collider = _resultBuffer[idx];
            if (!collider.isTrigger)
            {
                ledge = collider;
                break;
            }
        }

        if(ledge == null)
        {
            return false;
        }

        count = Physics2D.OverlapAreaNonAlloc(freeSpaceLowerBound, _detectionBoxP2.transform.position, _resultBuffer, _whatIsGround);
        for (var idx = 0; idx < count; idx++)
        {
            var collider = _resultBuffer[idx];
            if (!collider.isTrigger)
            {
                ledge = null;
                return false;
            }
        }

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        if (_detectionBoxP1 == null || _detectionBoxP2 == null)
        {
            return;
        }
        var ledgeDetectorUpperBound = LedgeDetectorUpperBound;
        var freeSpaceLowerBound = FreeSpaceLowerBound;

        Gizmos.color = Color.red;
        //var width = ledgeDetectorUpperBound.x - _detectionBoxP1.position.x;
        //var height = LedgeDetectorUpperBound.y - _detectionBoxP1.position.y;
        Gizmos.DrawWireCube(
            new Vector2(_detectionBoxP1.position.x + LedgeDetectorWidth / 2, _detectionBoxP1.position.y + LedgeDetectorHeight / 2),
            new Vector2(LedgeDetectorWidth, LedgeDetectorHeight));

        Gizmos.color = Color.green;
        //var height = _detectionBoxP2.position.y - freeSpaceLowerBound.y;
        Gizmos.DrawWireCube(
            new Vector2(FreeSpaceLowerBound.x + FreeSpaceWidth / 2, FreeSpaceLowerBound.y + FreeSpaceHeight / 2),
            new Vector2(FreeSpaceWidth, FreeSpaceHeight));
    }
}
