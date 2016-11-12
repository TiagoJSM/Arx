using CommonInterfaces.Controllers;
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
    [CreateAssetMenu(fileName = "ChainThrow", menuName = "Weapons/Create Chain Throw Weapon", order = 1)]
    public class ChainThrow : BaseWeapon, IChainThrowWeapon
    {
        private float _focusTime;
        private List<ICharacter> _attackedEnemies;
        private ChainedProjectile _instantiatedHeldProjectile;

        [SerializeField]
        private int _damage;

        private ChainedProjectile InstantiatedHeldProjectile
        {
            get
            {
                if(_instantiatedHeldProjectile == null)
                {
                    _instantiatedHeldProjectile = Instantiate(projectile);
                    _instantiatedHeldProjectile.Origin = this.RightHandSocket;
                    _instantiatedHeldProjectile.transform.parent = null;
                    _instantiatedHeldProjectile.OnAttackFinish += OnAttackFinishHandler;
                }
                return _instantiatedHeldProjectile;
            }
        }

        public event Action OnAttackFinish;

        public ChainedProjectile projectile;

        public bool ReadyToThrow { get { return InstantiatedHeldProjectile.Status == ProjectileStatus.None; } }

        public ChainThrow()
        {
            WeaponType = WeaponType.ChainedProjectile;
        }

        public void Spin()
        {
            StartAttack();
        }

        public void FocusThrow()
        {
            _focusTime += Time.deltaTime;
        }

        public void Throw(float degrees, LayerMask enemyLayer, GameObject attacker)
        {
            if (!ReadyToThrow)
            {
                return;
            }
            StartAttack();
            InstantiatedHeldProjectile.Throw(degrees, enemyLayer, attacker, _damage);
        }

        void Awake()
        {
            _attackedEnemies = new List<ICharacter>();
        }

        void OnDestroy()
        {
            if(_instantiatedHeldProjectile != null)
            {
                _instantiatedHeldProjectile.OnAttackFinish -= OnAttackFinishHandler;
            }
        }

        private void OnAttackFinishHandler()
        {
            AttackFinished();
        }

        private void StartAttack()
        {
            _instantiatedHeldProjectile.Reset();
            _focusTime = 0;
        }

        private void AttackFinished()
        {
            _attackedEnemies.Clear();
            OnAttackFinish?.Invoke();
        }
    }
}
