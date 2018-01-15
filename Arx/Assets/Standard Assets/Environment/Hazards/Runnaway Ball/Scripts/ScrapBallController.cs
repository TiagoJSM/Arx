using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Hazards.Runnaway_Ball.Scripts
{
    public class ScrapBallController : MonoBehaviour
    {
        private Vector3 _previousPosition;

        private void Update()
        {
            var movement = transform.position - _previousPosition;
            var distance = Vector3.Distance(_previousPosition, transform.position);
            var speed = (-movement.x) * 10/** Time.deltaTime * 100*/;

            transform.rotation = Quaternion.Euler(0, 0, speed + transform.rotation.eulerAngles.z);

            _previousPosition = transform.position;
        }
    }
}
