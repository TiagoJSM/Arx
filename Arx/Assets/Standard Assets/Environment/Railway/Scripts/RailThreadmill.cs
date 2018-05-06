using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Railway.Scripts
{
    public class RailThreadmill : MonoBehaviour
    {
        [SerializeField]
        private Transform _rail;
        [SerializeField]
        private float _horizontalVelocity = -4.0f;

        private void Update()
        {
            _rail.position = _rail.position + new Vector3(_horizontalVelocity * Time.deltaTime, 0.0f);
        }
    }
}
