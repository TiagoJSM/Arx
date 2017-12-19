using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Enemies.Spider_Mine.Scripts
{
    public class ExplosivePart : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D _body;
        [SerializeField]
        private Collider2D _collider;
        [SerializeField]
        private Transform _centerOfExplosion;
        [SerializeField]
        private float _explosionForce = 20.0f;
        [SerializeField]
        private float _torque = 10.0f;

        public void Explode()
        {
            _collider.gameObject.transform.parent = null;
            _collider.enabled = true;

            var explosionDirection = new Vector2(_body.position.x - _centerOfExplosion.position.x, _body.position.y - _centerOfExplosion.position.y).normalized;
            if(Mathf.Approximately(explosionDirection.x, 0.0f))
            {
                explosionDirection.x = 0.2f;
            }

            var torqueDirection = -Mathf.Sign(explosionDirection.x);

            _body.gameObject.transform.parent = null;
            _body.WakeUp();
            _body.AddForce(explosionDirection * _explosionForce, ForceMode2D.Impulse);
            _body.AddTorque(torqueDirection * _torque, ForceMode2D.Impulse);
        }
    }
}
