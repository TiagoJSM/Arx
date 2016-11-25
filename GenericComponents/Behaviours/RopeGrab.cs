using MathHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Behaviours
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class RopeGrab : MonoBehaviour
    {
        private Rigidbody2D _body;
        private Rope _rope;
        private RopePart _currentRopePart;
        private Rigidbody2D _ropeBodyPart;
        private float _originalGravityScale;

        public Transform ropeGrab;
        public float hForce = 10;

        void Start()
        {
            _body = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            _currentRopePart = _rope.GetRopePartAt(this.transform.position);
            var closestSegment = _rope.GetClosestRopeSegment(this.transform.position);
            this.gameObject.transform.parent = _currentRopePart.transform;
            this.gameObject.transform.position = 
                FloatUtils.ClosestPointOnLine(closestSegment.Value.P1, closestSegment.Value.P2, this.transform.position);
            this.gameObject.transform.rotation = _currentRopePart.transform.rotation;

            var vertical = Input.GetAxis("Vertical");
            if (Mathf.Abs(vertical) > 0.01)
            {
                var move = vertical < 0 ? new Vector3(0, -0.2f) : new Vector3(0, 0.2f);
                this.transform.localPosition += move;
            }
            var horizontal = Input.GetAxis("Horizontal");
            //Mathf.LerpAngle
            if (Mathf.Abs(horizontal) > 0.01)
            {
                _currentRopePart.PhysicsRopePart.AddForce(new Vector2(hForce * horizontal, 0));
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if(_currentRopePart != null)
            {
                return;
            }
            if (_rope != null)
            {
                return;
            }
            var rope = collider.gameObject.GetComponent<Rope>();
            if (rope == null)
            {
                return;
            }
            _rope = rope;

            _currentRopePart = _rope.GetRopePartAt(this.transform.position);
            this.gameObject.transform.parent = _currentRopePart.transform;
            _originalGravityScale = _body.gravityScale;
            _body.gravityScale = 0;
            _body.velocity = Vector2.zero;
        }

        //void OnCollisionStay2D(Collision2D collision)
        //{
        //    var rope = collision.gameObject.GetComponent<Rope>();
        //    if (rope == null || rope != _rope)
        //    {
        //        return;
        //    }
        //    var contact = collision.contacts.First();
        //    var ropePart = _rope.GetRopePartAt(contact.point);
        //    _ropeBodyPart = _rope.GetRopePartRigidBodyAt(contact.point);
        //    if (ropePart == _currentRopePart)
        //    {
        //        return;
        //    }
        //    Debug.Log("stay");
        //    _currentRopePart = ropePart;
        //    this.gameObject.transform.parent = _currentRopePart.transform;
        //}

        //void OnCollisionExit2D(Collision2D collision)
        //{
        //    var rope = collision.gameObject.GetComponent<Rope>();
        //    if (rope == null || rope != _rope)
        //    {
        //        return;
        //    }

        //    this.gameObject.transform.parent = null;
        //    _body.gravityScale = _originalGravityScale;
        //}
    }
}
