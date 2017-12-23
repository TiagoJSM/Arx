using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.InventorySystem
{
    public class ItemGravity : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _hitMask;
        [SerializeField]
        private float _gravity;

        private void Update()
        {
            var position = transform.position;
            var movement = Time.deltaTime * _gravity;
            var hit = 
                Physics2D.Raycast(
                    position,
                    new Vector2(0, Mathf.Sign(_gravity)),
                    Mathf.Abs(movement),
                    _hitMask);

            if (hit)
            {
                movement = hit.point.y - position.y;
            }

            transform.position = new Vector3(position.x, position.y + movement, position.z);
        }
    }
}
