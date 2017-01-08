using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Hazards
{
    public class DamageOnTouch : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _platformMask;
        [SerializeField]
        private Transform _safeSpot;
        [SerializeField]
        private int _damage = 1;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!_platformMask.IsInAnyLayer(col.gameObject))
            {
                return;
            }

            var controller = col.GetComponent<MainPlatformerController>();
            if(controller == null)
            {
                return;
            }

            if(_safeSpot != null)
            {
                controller.Hit(gameObject, _safeSpot.position, _damage);
            }
            else
            {
                controller.Attacked(gameObject, _damage, transform.position);
            }
        }
    }
}
