using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts
{
    public class FixedRotation : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _fixedLocalEulerRotation;

        private void LateUpdate()
        {
            transform.rotation = Quaternion.Euler(_fixedLocalEulerRotation);
        }
    }
}
