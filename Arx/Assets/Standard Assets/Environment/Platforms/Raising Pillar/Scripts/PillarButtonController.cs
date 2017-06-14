using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;

namespace Assets.Standard_Assets.Environment.Platforms.Raising_Pillar.Scripts
{
    public class PillarButtonController : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _mask;
        [SerializeField]
        private RaisingPillar _pillar;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_mask.IsInAnyLayer(collision.gameObject))
            {
                _pillar.Raise();
            }
        }
    }
}
