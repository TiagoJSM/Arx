using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Extensions
{
    public static class Joint2DExtensions
    {
        public static IEnumerable<Rigidbody2D> GetRigidBodies2DInJointSequence(this Joint2D ropeEnd)
        {
            var bodies = new List<Rigidbody2D>();
            var joint = ropeEnd;

            while (true)
            {
                bodies.Add(joint.GetComponent<Rigidbody2D>());
                if (joint.connectedBody == null)
                {
                    break;
                }
                joint = joint.connectedBody.GetComponent<Joint2D>();
            }

            return bodies;
        }

        public static IEnumerable<HingeJoint2D> GetChainedHingeJoints(this HingeJoint2D joint)
        {
            var joints = new List<HingeJoint2D>();

            while (true)
            {
                joints.Add(joint);
                if (joint.connectedBody == null)
                {
                    break;
                }
                joint = joint.connectedBody.GetComponent<HingeJoint2D>();
                if(joint == null)
                {
                    break;
                }
            }

            return joints;
        }

    }
}
