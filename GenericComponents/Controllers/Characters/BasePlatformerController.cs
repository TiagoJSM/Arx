using GenericComponents.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Controllers.Characters
{
    public class BasePlatformerController : MonoBehaviour
    {
        private Collider2D activePlatformCollider;

        public Transform groundCheck;
        public LayerMask whatIsGround;
        public float groundCheckRadius = 0.2f;

        void OnCollisionEnter2D(Collision2D other)
        {
            if (activePlatformCollider != null)
            {
                return;
            }
            foreach (var contact in other.contacts)
            {
                if (contact.normal.y > 0.5)
                {
                    transform.parent = other.transform;
                    activePlatformCollider = contact.collider;
                    return;
                }
            }
            activePlatformCollider = null;
        }

        void OnCollisionExit2D(Collision2D other)
        {
            if (other.collider == activePlatformCollider)
            {
                transform.parent = null;
                activePlatformCollider = null;
            }
        }

        protected void Flip(Direction direction)
        {
            var scale = transform.localScale;
            if ((direction == Direction.Right && scale.x > 0) || (direction == Direction.Left && scale.x < 0))
            {
                return;
            }
            scale.x *= -1;
            transform.localScale = scale;
        }

        protected Direction DirectionOfMovement(float horizontal, Direction defaultValue)
        {
            if (horizontal > 0)
            {
                return Direction.Right;
            }
            else if (horizontal < 0)
            {
                return Direction.Left;
            }
            return defaultValue;
        }

        protected bool CheckGrounded()
        {
            return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
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
