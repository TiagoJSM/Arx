using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Enemies.Platform_Patrol.Scripts
{
    enum PlatformPatrolStartingPoint
    {
        Frist = 0,
        Two = 1,
        Third = 2,
        Fourth = 3
    }

    public class PlatformPatrol : MonoBehaviour
    {
        private Vector2[] _verts = new Vector2[4];
        private int _vertIdx;

        [SerializeField]
        private SpriteRenderer _sprite;
        [SerializeField]
        private float _velocity;
        [SerializeField]
        private bool _clockwork;
        [SerializeField]
        private PlatformPatrolStartingPoint _startingPoint;

        private void Start()
        {
            _vertIdx = (int)_startingPoint;
            var center = _sprite.bounds.center;
            var bOffset = new Vector2(center.x, center.y);
            var size = _sprite.bounds.size;

            _verts[0] = bOffset + new Vector2(-size.x, size.y) * 0.5f;
            _verts[1] = bOffset + new Vector2(size.x, size.y) * 0.5f;
            _verts[2] = bOffset + new Vector2(size.x, -size.y) * 0.5f;
            _verts[3] = bOffset + new Vector2(-size.x, -size.y) * 0.5f;

            if (!_clockwork)
            {
                var tmp = _verts[1];
                _verts[1] = _verts[3];
                _verts[3] = tmp;
            }

            StartCoroutine(MovementRoutine());
        }

        private IEnumerator MovementRoutine()
        {
            var nextIdx = _vertIdx + 1;
            var startTime = Time.time;

            if (nextIdx == _verts.Length)
            {
                nextIdx = 0;
            }

            var timeToTarget = TimeToTarget(_verts[_vertIdx], _verts[nextIdx]);

            while (true)
            {
                var delta = (Time.time - startTime) / timeToTarget;
                var position = Vector2.Lerp(_verts[_vertIdx], _verts[nextIdx], delta);
                transform.position = new Vector3(position.x, position.y, transform.position.z);

                yield return null;

                if (delta >= 1)
                {
                    startTime = Time.time;
                    _vertIdx++;
                    if (_vertIdx == _verts.Length)
                    {
                        _vertIdx = 0;
                    }
                    nextIdx = _vertIdx + 1;
                    if (nextIdx == _verts.Length)
                    {
                        nextIdx = 0;
                    }
                    timeToTarget = TimeToTarget(_verts[_vertIdx], _verts[nextIdx]);
                }
            }
        }

        private float TimeToTarget(Vector2 start, Vector2 target)
        {
            return Vector2.Distance(start, target) / _velocity;
        }
    }
}
