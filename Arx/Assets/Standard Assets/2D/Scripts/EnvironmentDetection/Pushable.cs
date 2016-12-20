using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.EnvironmentDetection
{
    public class Pushable : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D _body;

        public void Push(float force)
        {
            _body.AddForce(new Vector2(force, 0), ForceMode2D.Impulse);
        }
    }
}
