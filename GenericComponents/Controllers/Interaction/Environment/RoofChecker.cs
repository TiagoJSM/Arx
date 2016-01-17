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
        private int _roofColliders;

        public Collider2D roofDetector;
        public bool IsTouchingRoof { get { return _roofColliders != 0; } }

        void OnTriggerEnter2D(Collider2D other)
        {
            if(roofDetector == null)
            {
                return;
            }
            if(!Physics2D.IsTouching(roofDetector, other))
            {
                return;
            }

            _roofColliders++;
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (roofDetector == null)
            {
                return;
            }
            if(IsTouchingRoof != other)
            {
                return;
            }
            if (Physics2D.IsTouching(roofDetector, other))
            {
                return;
            }

            _roofColliders--;
        }
    }
}
