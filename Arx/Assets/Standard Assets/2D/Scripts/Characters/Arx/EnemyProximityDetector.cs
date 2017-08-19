using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Characters.Arx
{
    public class EnemyProximityDetector : MonoBehaviour
    {
        private Collider2D[] _enemies;

        [SerializeField]
        private LayerMask _enemyLayer;
        [SerializeField]
        private float _radius = 10;

        public bool EnemyNearby { get; private set; }

        public EnemyProximityDetector()
        {
            _enemies = new Collider2D[1];
        }

        private void Update()
        {
            var count = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, _enemies, _enemyLayer);
            EnemyNearby = count > 0;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
