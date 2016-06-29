using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components.Weapons
{
    public class Projectile : MonoBehaviour
    {
        public Vector3 direction;
        public float speed;
        public float lifetime = 4;

        void Update()
        {
            this.transform.position += direction * speed;
            lifetime -= Time.deltaTime;
            if(lifetime <= 0)
            {
                Destroy(this.gameObject);
            }
        }

        void OnTriggerEnter2D()
        {
            Destroy(this.gameObject);
        }
    }
}
