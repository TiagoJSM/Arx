using Assets.Standard_Assets._2D.Scripts.Footsteps;
using GenericComponents.Behaviours;
using MathHelper;
using System;
using System.Collections;
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
        private MaterialRopeMovementPlayer _ropeMovementPlayer;
        private Coroutine _grabRope;

        [SerializeField]
        private float _grabLadderTime = 0.2f;
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

            _ropeMovementPlayer.GrabRope(rope.gameObject);
            CurrentRopePart = Rope.GetRopePartAt(this.transform.position);
            StopCoroutine();
            _grabRope = StartCoroutine(GrabRopeRoutine());
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
            var transform = this.gameObject.transform;
            transform.parent = CurrentRopePart.transform;
            transform.position =
                FloatUtils.ClosestPointOnLine(closestSegment.Value.P1, closestSegment.Value.P2, transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, CurrentRopePart.transform.rotation, Time.deltaTime * 15.0f);

            if (Mathf.Abs(vertical) > 0.01)
            {
                var move = new Vector3(0, _ropeVerticalSpeed * Time.deltaTime * Mathf.Sign(vertical));
                var size = Rope.GetRopeSizeEndingIn(transform.position);
                var sizeAfterMove = size - move.y;

                if (CanClimpRope(sizeAfterMove, vertical))
                {
                    transform.localPosition += move;
                    CurrentRopePart = Rope.GetRopePartAt(transform.position);
                    transform.parent = CurrentRopePart.transform;
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
            _ropeMovementPlayer = GetComponent<MaterialRopeMovementPlayer>();
        }

        private void FixedUpdate()
        {
            if (CurrentRopePart != null && _horizontalRopeMovement != 0)
            {
                CurrentRopePart.PhysicsRopePart.AddForce(new Vector2(_horizontalRopeMovement, 0));
            }
        }

        private IEnumerator GrabRopeRoutine()
        {
            var elapsed = 0.0f;
            Vector2 start = transform.position;
            Vector2 grabPosition = CurrentRopePart.transform.position;
            while (elapsed < _grabLadderTime)
            {
                elapsed += Time.deltaTime;
                var currentPosition = Vector2.Lerp(start, grabPosition, elapsed / _grabLadderTime);
                transform.position = new Vector3(currentPosition.x, currentPosition.y, transform.position.z);
                yield return null;
            }
            _grabRope = null;
        }

        private void StopCoroutine()
        {
            if (_grabRope != null)
            {
                StopCoroutine(_grabRope);
            }
        }
    }
}
