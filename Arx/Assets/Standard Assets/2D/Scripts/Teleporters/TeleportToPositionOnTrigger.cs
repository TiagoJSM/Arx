using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TeleportToPositionOnTrigger : BaseTeleportToPosition
{
    [SerializeField]
    private LayerMask _layerMask;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(_layerMask.IsInAnyLayer(collider.gameObject))
        {
            Teleport(collider.gameObject);
        }
    }
}

