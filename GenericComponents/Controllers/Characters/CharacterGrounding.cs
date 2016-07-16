using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Controllers.Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterGrounding : MonoBehaviour
    {
        private Rigidbody2D _body;

        void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            if(_body.velocity.y > 0)
            {
                _body.velocity = new Vector2(_body.velocity.x, 0);
            }
        }
    }
}
