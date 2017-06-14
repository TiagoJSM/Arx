using Assets.Standard_Assets._2D.Scripts.Managers;
using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Hazards
{
    public class DeathZone : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _layerMask;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_layerMask.IsInAnyLayer(collision.gameObject))
            {
                LevelManager.Instance.RestartCurrentLevelFromCheckpoint();
            }
        }
    }
}
