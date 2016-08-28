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
    }
}
