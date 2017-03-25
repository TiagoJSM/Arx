using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Controllers
{
    public class LadderMovement : MonoBehaviour
    {
        [SerializeField]
        private float _verticalUpSpeed = 15;
        [SerializeField]
        private float _verticalDownSpeed = 20;
        [SerializeField]
        private PlatformerCharacterController _characterController;

        public void GrabLadder()
        {
            _characterController.ApplyMovementAndGravity = false;
        }

        public void MoveOnLadder(float vertical)
        {
            if (Mathf.Abs(vertical) <= 0.01)
            {
                return;
            }
            var direction = Mathf.Sign(vertical);
            var move =
                direction > 0
                    ? new Vector3(0, _verticalUpSpeed * Time.deltaTime * direction)
                    : new Vector3(0, _verticalDownSpeed * Time.deltaTime * direction);

            this.transform.localPosition += move;
        }

        public void LetGoLadder()
        {
            _characterController.ApplyMovementAndGravity = true;
        }
    }
}
