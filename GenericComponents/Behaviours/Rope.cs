using Extensions;
using MathHelper.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Behaviours
{
    public class Rope : BaseRope
    {
        private EdgeCollider2D _ropeCollider;
        private List<Rigidbody2D> _bodies;
        private Dictionary<Rigidbody2D, RopePart> _jointBodyToRopePart;

        [SerializeField]
        private Joint2D _ropeEnd;
        
        public override Vector2[] Points
        {
            get
            {
                return _ropeCollider.points;
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
                _ropeEnd = value;
            }
        }

        public Rigidbody2D GetRopePartRigidBodyAt(Vector2 point)
        {
            var segmentPairs = _bodies.ToPairs();
            var body = default(Rigidbody2D);
            var minDifference = float.MaxValue;
            foreach (var segment in segmentPairs)
            {
                var distanceTo1 = Vector2.Distance(point, segment.Item1.position);
                var distanceTo2 = Vector2.Distance(point, segment.Item2.position);
                var ropeDistance = Vector2.Distance(segment.Item1.position, segment.Item2.position);

                var pointTotalDistance = distanceTo1 + distanceTo2;
                var difference = pointTotalDistance - ropeDistance;
                if (minDifference > difference)
                {
                    body = segment.Item2;
                    minDifference = difference;
                }
            }

            return body;
        }

        public RopePart GetRopePartAt(Vector2 point)
        {
            var body = GetRopePartRigidBodyAt(point);

            if (body == null)
            {
                return null;
            }
            return _jointBodyToRopePart[body];
        }

        public float GetRopeSizeEndingIn(Vector2 point)
        {
            var body = GetRopePartRigidBodyAt(point);
            if(body == null)
            {
                return 0;
            }
            var bodyIdx = _bodies.IndexOf(body);
            var remainingBodies = _bodies.Count - bodyIdx;
            if(remainingBodies <= 0)
            {
                return 0;
            }
            var bodies = _bodies.GetRange(bodyIdx, remainingBodies);
            var bodyPairs = bodies.ToPairs();
            return bodyPairs.Sum(p => Vector2.Distance(p.Item1.transform.position, p.Item2.transform.position)) + 
                Vector2.Distance(point, bodies[0].transform.position);
        }

        public LineSegment2D? GetClosestRopeSegment(Vector2 point)
        {
            var segmentPairs = _bodies.ToPairs();
            var tuple = default(Tuple<Rigidbody2D, Rigidbody2D>?);
            var minDifference = float.MaxValue;
            foreach (var segment in segmentPairs)
            {
                var distanceTo1 = Vector2.Distance(point, segment.Item1.position);
                var distanceTo2 = Vector2.Distance(point, segment.Item2.position);
                var ropeDistance = Vector2.Distance(segment.Item1.position, segment.Item2.position);

                var pointTotalDistance = distanceTo1 + distanceTo2;
                var difference = pointTotalDistance - ropeDistance;
                if (minDifference > difference) //ToDo: don't hardcode this
                {
                    tuple = segment;
                    minDifference = difference;
                }
            }

            if(tuple == null)
            {
                return null;
            }

            return new LineSegment2D(tuple.Value.Item1.position, tuple.Value.Item2.position);
        }

        public void RefreshRope(Joint2D ropeEnd)
        {
            _ropeEnd = ropeEnd;

            _ropeCollider = this.gameObject.AddComponent<EdgeCollider2D>();
            _ropeCollider.isTrigger = true;

            PopulateRopeCollider();
            AddRopeParts();
        }

        void Start()
        {
            RefreshRope(_ropeEnd);
        }

        void FixedUpdate()
        {
            UpdateCollider();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            var joints = GetComponentsInChildren<Joint2D>().Where(j => j.enabled).ToArray();
            var pairs = joints.Select(j => j.transform).ToPairs();
            foreach(var pair in pairs)
            {
                Gizmos.DrawLine(pair.Item1.transform.position, pair.Item2.transform.position);
            }
            foreach (var joint in joints)
            {
                Gizmos.DrawSphere(joint.transform.position, 0.5f);
            }
            
        }

        private void AddRopeParts()
        {
            _jointBodyToRopePart = new Dictionary<Rigidbody2D, RopePart>();
            foreach (var body in _bodies)
            {
                var go = new GameObject(body.name + "Part", typeof(RopePart));
                go.transform.parent = this.transform;
                var ropePart = go.GetComponent<RopePart>();
                ropePart.PhysicsRopePart = body;
                _jointBodyToRopePart.Add(body, ropePart);
            }
            
        }

        private void PopulateRopeCollider()
        {
            if (_ropeEnd == null || _ropeEnd.connectedBody == null)
            {
                return;
            }
            _bodies = _ropeEnd.GetRigidBodies2DInJointSequence().ToList();

            UpdateCollider();
        }

        private void UpdateCollider()
        {
            var ropePosition = transform.position.ToVector2();
            _ropeCollider.points = _bodies.Select(b => b.position - ropePosition).ToArray();
        }
    }
}
