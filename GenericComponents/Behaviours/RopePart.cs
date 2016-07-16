using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Behaviours
{
    public class RopePart : MonoBehaviour
    {
        public Rigidbody2D PhysicsRopePart { get; set; }
        public Transform PhysicsRopePartTransform
        {
            get
            {
                return PhysicsRopePart.transform;
            }
        }

        void FixedUpdate()
        {
            this.transform.position = PhysicsRopePart.transform.position;
            this.transform.rotation = PhysicsRopePart.transform.rotation;
            //this.transform.localScale = PhysicsRopePart.transform.localScale;
        }
    }
}
