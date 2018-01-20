using System;
using System.Collections;
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
        private Coroutine _grabLadder;

        [SerializeField]
        private float _grabLadderTime = 0.2f;
        [SerializeField]
        private float _verticalUpSpeed = 15;
        [SerializeField]
        private float _verticalDownSpeed = 20;
        [SerializeField]
        private PlatformerCharacterController _characterController;

        public void GrabLadder(GameObject ladder)
        {
            StopCoroutine();
            _grabLadder = StartCoroutine(GrabLadderRoutine(ladder.transform.position.x));
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
            StopCoroutine();
        }

        private void Awake()
        {
           _controller = GetComponent<CharacterController2D>();
        }

        private IEnumerator GrabLadderRoutine(float xGrab)
        {
            var elapsed = 0.0f;
            var startX = transform.position.x;
            while (elapsed < _grabLadderTime)
            {
                elapsed += Time.deltaTime;
                var currentX = Mathf.Lerp(startX, xGrab, elapsed / _grabLadderTime);
                var position = transform.position;
                position.x = currentX;
                transform.position = position;
                yield return null;
            }
            _grabLadder = null;
        }

        private void StopCoroutine()
        {
            if(_grabLadder != null)
            {
                StopCoroutine(_grabLadder);
            }
        }
    }
}
