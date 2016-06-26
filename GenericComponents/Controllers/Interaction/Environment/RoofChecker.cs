using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;

namespace GenericComponents.Controllers.Interaction.Environment
{
    public class RoofChecker : MonoBehaviour
    {
        private List<Collider2D> _colliders;

        public Collider2D roofDetector;
        public LayerMask whatIsGround;

        public bool IsTouchingRoof { get { return _colliders.Count != 0; } }

        void Start()
        {
            _colliders = new List<Collider2D>();
        }

        void FixedUpdate()
        {
            if(_colliders.Any(c => c == null))
            {
                _colliders = _colliders.Where(c => c != null).ToList();
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsValid(other))
            {
                return;
            }
            if (Physics2D.IsTouching(roofDetector, other))
            {
                _colliders.AddIfDoesntContain(other);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (!IsValid(other))
            {
                return;
            }
            if (!Physics2D.IsTouching(roofDetector, other))
            {
                _colliders.Remove(other);
            }
        }

        private bool IsValid(Collider2D other)
        {
            if (other.isTrigger)
            {
                return false;
            }
            if (roofDetector == null)
            {
                return false;
            }
            if (!whatIsGround.IsInAnyLayer(other.gameObject))
            {
                return false;
            }
            return true;
        }
    }
}
