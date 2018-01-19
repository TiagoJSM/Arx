using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Controllers
{
    [RequireComponent(typeof(CharacterController2D))]
    public class LadderMovement : MonoBehaviour
    {
        private CharacterController2D _controller;

        [SerializeField]
        private float _verticalUpSpeed = 15;
        [SerializeField]
        private float _verticalDownSpeed = 20;
        [SerializeField]
        private PlatformerCharacterController _characterController;

        public void GrabLadder()
        {
        }

        public void MoveOnLadder(float vertical)
        {
            if (Mathf.Abs(vertical) <= 0.01)
            {
                _controller.move(Vector3.zero);
                return;
            }
            var direction = Mathf.Sign(vertical);
            var move =
                direction > 0
                    ? new Vector3(0, _verticalUpSpeed * Time.deltaTime * direction)
                    : new Vector3(0, _verticalDownSpeed * Time.deltaTime * direction);

            _controller.move(move);
        }

        public void LetGoLadder()
        {
        }

        private void Awake()
        {
           _controller = GetComponent<CharacterController2D>();
        }
    }
}
