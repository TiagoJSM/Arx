using CommonInterfaces.Controllers;
using Extensions;
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
        [SerializeField]
        private LayerMask _colliderLayer;

        public LayerMask EnemyLayer { get; set; }
        public int Damage { get; set; }
        public GameObject Attacker { get; set; }

        void Update()
        {
            this.transform.position += direction * (speed * Time.deltaTime);
            lifetime -= Time.deltaTime;
            if(lifetime <= 0)
            {
                Destroy(this.gameObject);
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.isTrigger)
            {
                return;
            }
            if (_colliderLayer.IsInAnyLayer(other.gameObject))
            {
                Destroy(this.gameObject);
                return;
            }
            if (!EnemyLayer.IsInAnyLayer(other.gameObject))
            {
                return;
            }
            var character = other.gameObject.GetComponent<ICharacter>();
            if(character == null)
            {
                return;
            }
            character.Attacked(Attacker, Damage, this.transform.position, DamageType.Shoot);
            Destroy(this.gameObject);
        }
    }
}
