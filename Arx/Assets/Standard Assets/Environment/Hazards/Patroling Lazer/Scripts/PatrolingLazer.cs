using Assets.Standard_Assets.GenericComponents;
using Assets.Standard_Assets.GenericComponents.Waypoint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Hazards.Patroling_Lazer.Scripts
{
    public class PatrolingLazer : MonoBehaviour
    {
        private int _idx = 1;
        private Vector2 _previous;
        private Vector2 _next;

        [SerializeField]
        private WaypointPath _path;
        [SerializeField]
        private float _rotation = 5;
        [SerializeField]
        private float _speed = 10;

        private void Start()
        {
            _previous = _path[0];
            _next = _path[1];
            transform.position = _previous;
            StartCoroutine(PatrolRoutine());
        }

        private IEnumerator PatrolRoutine()
        {
            while (true)
            {
                yield return MovementBetweenPoints();
                _idx++;
                if(_idx >= _path.Count())
                {
                    _idx = 0;
                }
                _previous = _next;
                _next = _path[_idx];
            }
        }

        private IEnumerator MovementBetweenPoints()
        {
            var time = Vector2.Distance(_previous, _next) / _speed;
            var elapsed = 0.0f;
            while (elapsed <= time)
            {
                transform.position = Vector2.Lerp(_previous, _next, elapsed / time);
                transform.localRotation = 
                    Quaternion.Euler(0, 0, _rotation * Time.deltaTime + transform.localRotation.eulerAngles.z);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}
