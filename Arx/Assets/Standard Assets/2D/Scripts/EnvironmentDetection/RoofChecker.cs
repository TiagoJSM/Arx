using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RoofChecker : MonoBehaviour
{
    private Collider2D[] _colliderBuffer = new Collider2D[10];

    [SerializeField]
    private LayerMask _whatIsGround;
    [SerializeField]
    private Transform _roofAreaP1;
    [SerializeField]
    private Transform _roofAreaP2;

    public bool IsTouchingRoof
    {
        get
        {
            return CheckRoof();
        }
    }

    private bool CheckRoof()
    {
        if(_roofAreaP1 == null || _roofAreaP2 == null)
        {
            return false;
        }
        var count = 
            Physics2D
                .OverlapAreaNonAlloc(
                    _roofAreaP1.position, 
                    _roofAreaP2.position, 
                    _colliderBuffer, 
                    _whatIsGround);

        for(var idx = 0; idx < count; idx++)
        {
            var collider = _colliderBuffer[idx];
            if (!collider.isTrigger)
            {
                return true;
            }
        }
        return false;
    }
}
