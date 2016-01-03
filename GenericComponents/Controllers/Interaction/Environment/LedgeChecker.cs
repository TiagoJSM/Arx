using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;

namespace GenericComponents.Controllers.Interaction.Environment
{
    public class LedgeChecker : MonoBehaviour
    {
        private readonly Color GrabableConditionColor = Color.green;
        private readonly Color NotGrabableConditionColor = Color.red;

        private Vector2 GrabableLedgeCornerOffset
        {
            get
            {
                return grabableLedgeCorner + transform.position.ToVector2();
            }
        }

        public Vector2 grabableLedgeCorner;
        public float size = 1.0f;
        public LayerMask whatIsGround;

        public bool IsLedgeDetected(out Collider2D ledge)
        {
            ledge = LedgeArea();
            if (ledge == null)
            {
                return false;
            }
            return AreaOverLedge() == null;
        }

        void OnDrawGizmosSelected()
        {
            var grabableLedgeCornerOffset = GrabableLedgeCornerOffset;
            var emptySpaceOverLedgeCenter = grabableLedgeCornerOffset;
            var ledgeDetectorCenter = grabableLedgeCornerOffset;
            ledgeDetectorCenter.x += size / 2;
            ledgeDetectorCenter.y += size / 2;

            emptySpaceOverLedgeCenter.x += size / 2;
            emptySpaceOverLedgeCenter.y += size + (size / 2);

            Gizmos.color = LedgeArea() != null ? GrabableConditionColor : NotGrabableConditionColor;
            Gizmos.DrawWireCube(ledgeDetectorCenter.ToVector3(), new Vector3(size, size, size));
            Gizmos.color = AreaOverLedge() == null ? GrabableConditionColor : NotGrabableConditionColor;
            Gizmos.DrawWireCube(emptySpaceOverLedgeCenter.ToVector3(), new Vector3(size, size, size));
        }

        private Collider2D LedgeArea()
        {
            var point = GrabableLedgeCornerOffset;
            return Physics2D.OverlapArea(point, point + new Vector2(size, size), whatIsGround);
        }

        private Collider2D AreaOverLedge()
        {
            var emptySpaceOnTopOfLedge = GrabableLedgeCornerOffset;
            emptySpaceOnTopOfLedge.y += size;
            return Physics2D.OverlapArea(emptySpaceOnTopOfLedge, emptySpaceOnTopOfLedge + new Vector2(size, size), whatIsGround);
        }
    }
}
