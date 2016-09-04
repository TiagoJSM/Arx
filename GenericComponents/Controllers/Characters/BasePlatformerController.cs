using CommonInterfaces.Enums;
using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utils;

namespace GenericComponents.Controllers.Characters
{
    public class BasePlatformerController : CharacterController
    {
        private Collider2D activePlatformCollider;

        private Direction _direction;

        public Transform groundCheck;
        public LayerMask whatIsGround;
        public float groundCheckRadius = 0.2f;

        public Direction Direction
        {
            get
            {
                return _direction;
            }
        }

        protected float GroundContactMaxNormal
        {
            get
            {
                return 0.5f;
            }
        }

        /*void OnCollisionEnter2D(Collision2D other)
        {
            if (activePlatformCollider != null)
            {
                return;
            }
            foreach (var contact in other.contacts)
            {
                if (contact.normal.y > GroundContactMaxNormal)
                {
                    transform.parent = other.transform;
                    transform.rotation = Quaternion.identity;
                    activePlatformCollider = contact.collider;
                    return;
                }
            }
            activePlatformCollider = null;
        }*/

        protected override void Awake()
        {
            base.Awake();
            _direction = DirectionOfMovement(transform.localScale.x, Direction.Left);
        }

        /*void OnCollisionExit2D(Collision2D other)
        {
            if (other.collider == activePlatformCollider)
            {
                transform.parent = null;
                activePlatformCollider = null;
            }
        }*/

        protected void Flip(Direction direction)
        {
            var globalScale = transform.lossyScale;
            if ((direction == Direction.Right && globalScale.x > 0) || (direction == Direction.Left && globalScale.x < 0))
            {
                return;
            }
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            _direction = direction;
        }

        protected Direction DirectionOfMovement(float horizontal, Direction defaultValue)
        {
            return MovementUtils.DirectionOfMovement(horizontal, defaultValue);
        }

        protected float DirectionValue(Direction defaultValue)
        {
            return defaultValue.DirectionValue();
        }

        protected bool CheckGrounded()
        {
            return 
                Physics2D
                    .OverlapCircleAll(groundCheck.position, groundCheckRadius, whatIsGround)
                    .FirstOrDefault(c => !c.isTrigger);
        }

        protected void DrawGizmos()
        {
            if(groundCheck == null)
            {
                return;
            }
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
