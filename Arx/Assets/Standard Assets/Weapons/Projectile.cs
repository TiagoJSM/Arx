using Assets.Standard_Assets.Scripts;
using CommonInterfaces.Controllers;
using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Weapons
{
    public class Projectile : MonoBehaviour
    {
        private Vector3 _direction;
        public float speed;
        public float lifetime = 4;
        [SerializeField]
        private LayerMask _colliderLayer;
        [SerializeField]
        private LayerMask _enemyLayer;
        [SerializeField]
        private int _damage;
        [SerializeField]
        private AudioSource _hitSound;

        public LayerMask EnemyLayer
        {
            get
            {
                return _enemyLayer;
            }
            set
            {
                _enemyLayer = value;
            }
        }
        public int Damage
        {
            get
            {
                return _damage;
            }
            set
            {
                _damage = value;
            }
        }
        public GameObject Attacker { get; set; }
        public Vector3 Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
            }
        }

        void Update()
        {
            this.transform.position += Direction * (speed * Time.deltaTime);
            var angle = Vector2.Angle(Vector2.right, Direction);
            if (Direction.y < 0)
            {
                angle = -angle;
            }

            this.transform.rotation = Quaternion.Euler(0, 0, angle);
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
            var damage = character.Attacked(Attacker, Damage, this.transform.position, DamageType.Shoot, AttackTypeDetail.Generic, 1);
            if(damage > 0)
            {
                if (_hitSound != null)
                {
                    _hitSound.Play();
                }
            }
            
            Destroy(this.gameObject, 1);
        }
    }
}
