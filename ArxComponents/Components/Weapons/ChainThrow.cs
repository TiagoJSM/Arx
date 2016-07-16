using ArxGame.Components.EnemyControllers;
using CommonInterfaces.Enums;
using CommonInterfaces.Weapons;
using System;
using System.Collections;
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
        private ChainedProjectile _instantiatedHeldProjectile;

        public Collider2D detectionCollider;
        public ChainedProjectile projectile;

        public event Action OnAttackFinish;

        public GameObject Owner { get; set; }

        public WeaponType WeaponType
        {
            get
            {
                return WeaponType.ChainedProjectile;
            }
        }

        public bool ReadyToThrow { get; private set; }

        public void Spin()
        {
            StartAttack();
        }

        public void FocusThrow()
        {
            _focusTime += Time.deltaTime;
        }

        public void Throw(Direction direction)
        {
            ReadyToThrow = false;
            StartAttack();
            _instantiatedHeldProjectile.Throw(direction);
        }

        void Awake()
        {
            _attackedEnemies = new List<BaseEnemyController>();
            //this.enabled = false;
            //detectionCollider.enabled = false;
            _instantiatedHeldProjectile = Instantiate(projectile);
            _instantiatedHeldProjectile.origin = this.gameObject;
            _instantiatedHeldProjectile.transform.parent = null;
            _instantiatedHeldProjectile.OnAttackFinish += OnAttackFinishHandler;
        }

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
            //this.enabled = true;
            //detectionCollider.enabled = true;
            _focusTime = 0;
        }

        private void OnAttackFinishHandler()
        {
            ReadyToThrow = true;
            //_instantiatedHeldProjectile.Visible = true;
            //_instantiatedThrownProjectile.Visible = false;
            _attackedEnemies.Clear();
            OnAttackFinish?.Invoke();
        }
    }
}
