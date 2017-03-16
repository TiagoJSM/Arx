using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Hazards.Rotating_Ball.Scripts
{
    public class RotatingBall : MonoBehaviour
    {
        [SerializeField]
        private float rotationSpeed = 1;

        private void Update()
        {
            var currentRotation = transform.rotation.eulerAngles;
            transform.rotation = 
                Quaternion.Euler(
                    currentRotation.x, 
                    currentRotation.y, 
                    currentRotation.z + Time.deltaTime * rotationSpeed);
        }
    }
}
