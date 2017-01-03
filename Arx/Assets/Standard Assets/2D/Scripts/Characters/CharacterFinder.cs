using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using GenericComponents.Controllers.Characters;

namespace Assets.Standard_Assets._2D.Scripts.Characters
{
    public delegate void OnCharacterFound(BasePlatformerController controller);

    public class CharacterFinder : MonoBehaviour
    {
        private Collider2D[] _colliderBuffer = new Collider2D[10];

        public event OnCharacterFound OnCharacterFound;

        [SerializeField]
        private Transform _finderAreaP1;
        [SerializeField]
        private Transform _finderAreaP2;
        [SerializeField]
        private LayerMask _characterLayer;

        void Update()
        {
            if (OnCharacterFound == null)
            {
                return;
            }
            var count =
                Physics2D
                    .OverlapAreaNonAlloc(
                        _finderAreaP1.position,
                        _finderAreaP2.position,
                        _colliderBuffer,
                        _characterLayer);

            for (var idx = 0; idx < count; idx++)
            {
                var controller = _colliderBuffer[idx].GetComponent<BasePlatformerController>();
                if(controller != null)
                {
                    OnCharacterFound(controller);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if(_finderAreaP1 == null || _finderAreaP2 == null)
            {
                return;
            }
            var p1Transform = _finderAreaP1.transform;
            var p2Transform = _finderAreaP2.transform;
            var center = new Vector2(
                (p2Transform.position.x + p1Transform.position.x) / 2,
                (p2Transform.position.y + p1Transform.position.y) / 2);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(
                center,
                new Vector2(
                    Math.Abs(p2Transform.position.x - p1Transform.position.x), 
                    Math.Abs(p2Transform.position.y - p1Transform.position.y)));
        }
    }
}
