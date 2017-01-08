using GenericComponents.Controllers.Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Characters.CommonAiBehaviour
{
    public abstract class FlyingCharacterAiController : BaseCharacterAiController
    {
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

        public Vector2 StartingPosition
        {
            get
            {
                return _startingPosition;
            }
        }

        protected virtual void Awake()
        {
            _startingPosition = this.transform.position;
        }

        protected abstract void Move(Vector2 direction);

        protected void IddleMovement()
        {
            SetActiveCoroutine(IddleMovementCoroutine());
        }

        private IEnumerator IddleMovementCoroutine()
        {
            while (true)
            {
                _movementTarget = GetNewMovementPosition();
                yield return MoveTo(() => _movementTarget);
                var stopTime = UnityEngine.Random.Range(0, _maxStoppedIddleTime);
                yield return new WaitForSeconds(stopTime);
            }
        }

        private Vector2 GetNewMovementPosition()
        {
            var position =
                    new Vector2(
                        UnityEngine.Random.Range(-(patrolAreaWidth/2), (patrolAreaWidth / 2)),
                        UnityEngine.Random.Range(-(patrolAreaHeight/2), (patrolAreaHeight / 2)));
            position += _startingPosition.ToVector2();
            return position;
        } 

        protected Vector2 GetDirectionTo(Vector2 position)
        {
            var result = position - transform.position.ToVector2();
            return result.normalized;
        }

        protected IEnumerator MoveTo(Func<Vector2> targetPosition, Action onComplete = null)
        {
            var direction = GetDirectionTo(targetPosition());

            while (true)
            {
                direction = GetDirectionTo(targetPosition());
                Move(direction);
                yield return null;

                var distance = Vector2.Distance(targetPosition(), this.transform.position);
                if (distance <= _distanceFromPointThreshold)
                {
                    Move(Vector2.zero);
                    if(onComplete != null)
                    {
                        onComplete();
                    }
                    yield break;
                }
            }
        }

        protected virtual void OnDrawGizmosSelected()
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
