using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BaseTeleportToPosition : MonoBehaviour
{
    [SerializeField]
    private Transform _position;
    [SerializeField]
    private bool _ignoreZ = true;

    public void Teleport(GameObject teleportTarget)
    {
        var position = _position.position;
        if (_ignoreZ)
        {
            position.z = 0;
        }
        teleportTarget.transform.position = position;
    }
}

