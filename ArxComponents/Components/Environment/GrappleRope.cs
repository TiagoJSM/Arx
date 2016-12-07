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
        //private Rigidbody2D[] _bodies;
        private HingeJoint2D _ropeEnd;

        public override Vector2[] Points
        {
            get
            {
                var _bodies = _ropeEnd?.GetRigidBodies2DInJointSequence().ToArray();
                if (_bodies == null)
                {
                    return new Vector2[0];
                }
                return _bodies.Select(b => b.position).ToArray();
            }
        }

        public float RopeSize
        {
            get
            {
                var ropePoints = Points;
                if (ropePoints.Length < 2)
                {
                    return 0;
                }
                var size = 0f;
                var previousPoint = ropePoints[0];
                for (var idx = 1; idx < ropePoints.Length; idx++)
                {
                    size += Vector2.Distance(previousPoint, ropePoints[idx]);
                    previousPoint = ropePoints[idx];
                }
                return size;
            }
        }

        public HingeJoint2D RopeEnd
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
                    //_bodies = _ropeEnd?.GetRigidBodies2DInJointSequence().ToArray();
                }
            }
        }

        public HingeJoint2D[] Joints
        {
            get
            {
                return _ropeEnd.GetChainedHingeJoints().ToArray();
            }
        }

        private void FixedUpdate()
        {
            var connected = _ropeEnd.connectedBody;
            var endBody = _ropeEnd.GetComponent<Rigidbody2D>();
            endBody.rotation = connected.rotation;
        }
    }
}
