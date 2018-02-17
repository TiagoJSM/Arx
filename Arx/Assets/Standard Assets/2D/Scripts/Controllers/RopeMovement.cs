using Assets.Standard_Assets._2D.Scripts.Footsteps;
using GenericComponents.Behaviours;
using MathHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Controllers
{
    [RequireComponent(typeof(MaterialRopeMovementPlayer))]
    public class RopeMovement : MonoBehaviour
    {
        private float _horizontalRopeMovement;
        private MaterialRopeMovementPlayer _player;

        [SerializeField]
        private float _maxRopeHorizontalForce = 20;
        [SerializeField]
        private float _ropeVerticalSpeed = 4;
        [SerializeField]
        private float _minimumDistanceFromRopeOrigin = 1;

        public float RopeClimbDirection { get; private set; }
        public Rope Rope { get; private set; }
        public RopePart CurrentRopePart { get; private set; }

        public bool GrabRope(Rope rope)
        {
            Rope = rope;
            if (Rope == null)
            {
                return false;
            }

            _player.GrabRope(rope.gameObject);
            CurrentRopePart = Rope.GetRopePartAt(this.transform.position);
            this.gameObject.transform.parent = CurrentRopePart.transform;
            return true;
        }

        public void LetGoRope()
        {
            this.gameObject.transform.parent = null;
            CurrentRopePart = null;
            Rope = null;
        }

        public void MoveOnRope(float horizontal, float vertical)
        {
            RopeClimbDirection = 0;

            var closestSegment = Rope.GetClosestRopeSegment(this.transform.position);
            this.gameObject.transform.parent = CurrentRopePart.transform;
            this.gameObject.transform.position =
                FloatUtils.ClosestPointOnLine(closestSegment.Value.P1, closestSegment.Value.P2, this.transform.position);
            this.gameObject.transform.rotation = CurrentRopePart.transform.rotation;

            if (Mathf.Abs(vertical) > 0.01)
            {
                var move = new Vector3(0, _ropeVerticalSpeed * Time.deltaTime * Mathf.Sign(vertical));
                var size = Rope.GetRopeSizeEndingIn(this.transform.position);
                var sizeAfterMove = size - move.y;

                if (CanClimpRope(sizeAfterMove, vertical))
                {
                    this.transform.localPosition += move;
                    CurrentRopePart = Rope.GetRopePartAt(this.transform.position);
                    this.gameObject.transform.parent = CurrentRopePart.transform;
                    RopeClimbDirection = vertical > 0 ? 1 : -1;
                    return; //or we climb or we balance on the rope
                }
            }

            _horizontalRopeMovement = Mathf.Abs(horizontal) > 0.01f ? _maxRopeHorizontalForce * Math.Sign(horizontal) : 0;
        }

        private bool CanClimpRope(float ropeSizeAfterMove, float verticalMovement)
        {
            //if character is moving down movement is ok
            if (verticalMovement < 0)
            {
                return true;
            }
            //character can only move up if its at a distance from the rope top
            return _minimumDistanceFromRopeOrigin <= ropeSizeAfterMove;
        }

        private void Awake()
        {
            _player = GetComponent<MaterialRopeMovementPlayer>();
        }

        private void FixedUpdate()
        {
            if (CurrentRopePart != null && _horizontalRopeMovement != 0)
            {
                CurrentRopePart.PhysicsRopePart.AddForce(new Vector2(_horizontalRopeMovement, 0));
            }
        }
    }
}
