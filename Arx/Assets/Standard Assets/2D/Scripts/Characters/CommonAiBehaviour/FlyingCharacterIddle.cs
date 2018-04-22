using Assets.Standard_Assets.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Characters.CommonAiBehaviour
{
    [RequireComponent(typeof(CharacterController2D))]
    public class FlyingCharacterIddle : MonoBehaviour
    {
        private CharacterController2D _controller;
        private Vector3 _startingPosition;
        private Vector3 _movementTarget;

        [SerializeField]
        private float patrolAreaWidth = 5;
        [SerializeField]
        private float patrolAreaHeight = 5;
        [SerializeField]
        private float _distanceFromPointThreshold = 0.5f;
        [SerializeField]
        private float _maxStoppedIddleTime = 5;
        [SerializeField]
        private float _speed = 15.0f;

        public Vector2 StartingPosition
        {
            get
            {
                return _startingPosition;
            }
        }

        public IEnumerator IddleMovement()
        {
            while (true)
            {
                _movementTarget = GetNewMovementPosition();
                yield return CoroutineHelpers.MoveTo(_controller, () => _movementTarget, _speed);
                var stopTime = UnityEngine.Random.Range(0, _maxStoppedIddleTime);
                yield return new WaitForSeconds(stopTime);
            }
        }

        private void Awake()
        {
            _startingPosition = this.transform.position;
            _controller = GetComponent<CharacterController2D>();
        }

        private Vector2 GetNewMovementPosition()
        {
            var position =
                    new Vector2(
                        UnityEngine.Random.Range(-(patrolAreaWidth / 2), (patrolAreaWidth / 2)),
                        UnityEngine.Random.Range(-(patrolAreaHeight / 2), (patrolAreaHeight / 2)));
            position += _startingPosition.ToVector2();
            return position;
        }

        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying)
            {
                _startingPosition = this.transform.position;
            }

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(
                _startingPosition,
                new Vector2(patrolAreaWidth, patrolAreaHeight));

            if (Application.isPlaying)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, _movementTarget);
            }
        }
    }
}
