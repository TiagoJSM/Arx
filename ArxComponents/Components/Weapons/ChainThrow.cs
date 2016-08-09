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
    public class ChainThrow : MonoBehaviour, IChainThrowWeapon
    {
        private float _focusTime;
        private List<ICharacter> _attackedEnemies;
        private ChainedProjectile _instantiatedHeldProjectile;

        public event Action OnAttackFinish;

        public ChainedProjectile projectile;

        public WeaponType WeaponType
        {
            get
            {
                return WeaponType.ChainedProjectile;
            }
        }

        public bool ReadyToThrow { get { return _instantiatedHeldProjectile.Status == ProjectileStatus.None; } }

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
            if (!ReadyToThrow)
            {
                return;
            }
            StartAttack();
            _instantiatedHeldProjectile.Throw(direction);
        }

        void Awake()
        {
            _attackedEnemies = new List<ICharacter>();
            _instantiatedHeldProjectile = Instantiate(projectile);
            _instantiatedHeldProjectile.Origin = this.gameObject;
            _instantiatedHeldProjectile.transform.parent = null;
            _instantiatedHeldProjectile.OnAttackFinish += OnAttackFinishHandler;
        }

        private void OnAttackFinishHandler()
        {
            AttackFinished();
        }

        private void StartAttack()
        {
            _focusTime = 0;
        }

        private void AttackFinished()
        {
            _attackedEnemies.Clear();
            OnAttackFinish?.Invoke();
        }
    }
}
