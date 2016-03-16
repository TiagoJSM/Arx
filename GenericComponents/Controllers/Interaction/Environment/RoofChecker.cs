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
        public bool IsTouchingRoof { get { return _colliders.Count != 0; } }

        void Start()
        {
            _colliders = new List<Collider2D>();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.isTrigger)
            {
                return;
            }
            if (roofDetector == null)
            {
                return;
            }
            if(Physics2D.IsTouching(roofDetector, other))
            {
                _colliders.AddIfDoesntContain(other);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.isTrigger)
            {
                return;
            }
            if (roofDetector == null)
            {
                return;
            }
            if (!Physics2D.IsTouching(roofDetector, other))
            {
                _colliders.Remove(other);
            }
        }
    }
}
