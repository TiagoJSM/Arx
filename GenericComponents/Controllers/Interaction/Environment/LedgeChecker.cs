using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;

namespace GenericComponents.Controllers.Interaction.Environment
{
    public class LedgeChecker : MonoBehaviour
    {
        private Collider2D _ledge;
        //[SerializeField]
        private List<Collider2D> _ledgeColliders;
        //[SerializeField]
        private List<Collider2D> _freeSpaceColliders;

        public Collider2D ledgeDetector;
        public Collider2D freeSpaceDetector;
        public LayerMask whatIsGround;

        void Start()
        {
            _ledgeColliders = new List<Collider2D>();
            _freeSpaceColliders = new List<Collider2D>();
        }

        void FixedUpdate()
        {
            if (_ledgeColliders.Any(c => c == null))
            {
                _ledgeColliders = _ledgeColliders.Where(c => c != null).ToList();
            }
            if (_freeSpaceColliders.Any(c => c == null))
            {
                _freeSpaceColliders = _freeSpaceColliders.Where(c => c != null).ToList();
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsValid(other))
            {
                return;
            }
            if (Physics2D.IsTouching(ledgeDetector, other))
            {
                _ledge = other;
                _ledgeColliders.AddIfDoesntContain(other);
            }
            if (Physics2D.IsTouching(freeSpaceDetector, other))
            {
                _freeSpaceColliders.AddIfDoesntContain(other);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (!IsValid(other))
            {
                return;
            }
            if (!Physics2D.IsTouching(ledgeDetector, other))
            {
                _ledgeColliders.Remove(other);
            }
            if (!Physics2D.IsTouching(freeSpaceDetector, other))
            {
                _freeSpaceColliders.Remove(other);
            }
        }

        public bool IsLedgeDetected(out Collider2D ledge)
        {
            ledge = null;
            if (_ledgeColliders.Count != 0 && _freeSpaceColliders.Count == 0)
            {
                ledge = _ledge;
                return true;
            }
            return false;
        }

        private bool IsValid(Collider2D other)
        {
            if (other.isTrigger)
            {
                return false;
            }
            if (ledgeDetector == null || freeSpaceDetector == null)
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
