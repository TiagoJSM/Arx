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
    public class DefensiveFlightAroundTarget : MonoBehaviour
    {
        private CharacterController2D _controller;

        [SerializeField]
        private float _patrolDimension = 2;
        [SerializeField]
        private float _duration = 2.0f;
        [SerializeField]
        private float _speed = 25.0f;

        public IEnumerator FlyAround()
        {
            var origin = transform.position;
            var elapsed = 0.0f;
            while (elapsed < _duration)
            {
                var movementTarget = GetNewMovementPosition(origin);
                var startTime = Time.time;
                yield return CoroutineHelpers.MoveTo(_controller, () => movementTarget, _speed);
                var cycleElapsed = Time.time - startTime;
                elapsed += cycleElapsed;
            }
        }

        private void Awake()
        {
            _controller = GetComponent<CharacterController2D>();
        }

        private Vector2 GetNewMovementPosition(Vector3 origin)
        {
            var position =
                    new Vector2(
                        UnityEngine.Random.Range(-(_patrolDimension / 2), (_patrolDimension / 2)),
                        UnityEngine.Random.Range(-(_patrolDimension / 2), (_patrolDimension / 2)));
            position += origin.ToVector2();
            return position;
        }
    }
}
