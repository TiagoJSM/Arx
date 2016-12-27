using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.EnvironmentDetection
{
    public class Pushable : MonoBehaviour
    {
        private float? _force;

        [SerializeField]
        private Rigidbody2D _body;

        public void Push(float force)
        {
            _force = force;
        }

        private void FixedUpdate()
        {
            if(_force != null)
            {
                _body.AddForce(new Vector2(_force.Value, 0), ForceMode2D.Impulse);
            }
            _force = null;
        }
    }
}
