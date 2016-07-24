using Extensions;
using GenericComponents.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components.Environment
{
    public class GrappleRope : BaseRope
    {
        private Rigidbody2D[] _bodies;
        private Joint2D _ropeEnd;

        public override Vector2[] Points
        {
            get
            {
                if(_bodies == null)
                {
                    return new Vector2[0];
                }
                return _bodies.Select(b => b.position).ToArray();
            }
        }

        public Joint2D RopeEnd
        {
            get
            {
                return _ropeEnd;
            }
            set
            {
                if(_ropeEnd != value)
                {
                    _ropeEnd = value;
                    _bodies = _ropeEnd?.GetRigidBodies2DInJointSequence().ToArray();
                }
            }
        }
    }
}
