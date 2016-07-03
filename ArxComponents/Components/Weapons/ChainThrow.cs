using ArxGame.Components.EnemyControllers;
using CommonInterfaces.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components.Weapons
{
    public class ChainThrow : MonoBehaviour, IChainThrowWeapon
    {
        private float _focusTime;
        private List<BaseEnemyController> _attackedEnemies;
        private ChainedProjectile _instantiatedProjectile;

        public Collider2D detectionCollider;
        public ChainedProjectile projectile;

        public GameObject Owner { get; set; }

        public void Spin()
        {
            StartAttack();
        }

        public void FocusThrow()
        {
            _focusTime += Time.deltaTime;
        }

        public void Throw()
        {
            StartAttack();
            _instantiatedProjectile.Throw();
        }

        public void AttackIsOver()
        {
            _attackedEnemies.Clear();
            this.enabled = false;
            //detectionCollider.enabled = false;
        }

        void Awake()
        {
            _attackedEnemies = new List<BaseEnemyController>();
            //this.enabled = false;
            //detectionCollider.enabled = false;
            _instantiatedProjectile = Instantiate(projectile);
            _instantiatedProjectile.origin = this.gameObject;
            
            //_instantiatedProjectile.transform.parent = this.transform;
        }

        void Start()
        {
            _instantiatedProjectile.transform.position = this.transform.position;
        }

        //private void Update()
        //{
        //    _instantiatedProjectile.Throw();
        //}

        void OnTriggerEnter2D(Collider2D other)
        {
            var enemy = other.GetComponent<BaseEnemyController>();
            if (enemy == null)
            {
                return;
            }
            if (_attackedEnemies.Contains(enemy))
            {
                return;
            }
            _attackedEnemies.Add(enemy);
            enemy.Attacked(Owner, 10, null);
        }

        private void StartAttack()
        {
            this.enabled = true;
            //detectionCollider.enabled = true;
            _focusTime = 0;
        }
    }
}
