using Assets.Standard_Assets.Scripts;
using Assets.Standard_Assets.Weapons;
using CommonInterfaces.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Enemies.Guardian_Eye.Scripts
{
    public enum Phase
    {
        Phase1,
        Phase2,
        Phase3
    }

    public delegate void OnChargedShotFired(GuardianEye character);
    public delegate void OnChargedShotCharged(GuardianEye character);
    public delegate void OnDamaged(GuardianEye character, int damage);
    public delegate void OnKilled(GuardianEye character);

    public class GuardianEye : MonoBehaviour, ICharacter
    {
        private const int Max_Life_Points = 3;

        [SerializeField]
        private float _phase1Speed = 2;
        [SerializeField]
        private float _phase2Speed = 3;
        [SerializeField]
        private float _phase3Speed = 4;
        [SerializeField]
        private float _chargeTime = 3;
        [SerializeField]
        private float _chargeShootingTime = 1;
        [SerializeField]
        private Projectile _projectilePrefab;
        [SerializeField]
        private LayerMask _enemyLayer;
        [SerializeField]
        private Transform _projectileSpawnPosition;
        [SerializeField]
        private Transform _character;
        [SerializeField]
        private Transform _top;
        [SerializeField]
        private Transform _bottom;
        [SerializeField]
        private GameObject _chargedBeam;
        [SerializeField]
        private float _regenerationSpeed = 10;
        [SerializeField]
        private Sprite _defaultSprite;
        [SerializeField]
        private Sprite _regeneratingSprite;
        [SerializeField]
        private SpriteRenderer _renderer;

        private int _lifePoints = Max_Life_Points;
        private Coroutine _movementRoutine;
        private Coroutine _chargeShotCoroutine;

        private float Speed
        {
            get
            {
                switch (Phase)
                {
                    case Phase.Phase1: return _phase1Speed;
                    case Phase.Phase2: return _phase2Speed;
                    case Phase.Phase3: return _phase3Speed;
                    default: return _phase3Speed;
                }
            }
        }

        public event OnChargedShotCharged OnChargedShotCharged;
        public event OnChargedShotFired OnChargedShotFired;
        public event OnDamaged OnDamaged;
        public event OnKilled OnKilled;

        public bool CanBeAttacked { get; private set; }
        public bool IsMoving { get { return _movementRoutine != null; } }
        public Phase Phase { get; private set; }
        public Transform Top
        {
            get
            {
                return _top;
            }
        }
        public Transform Bottom
        {
            get
            {
                return _bottom;
            }
        }

        public GameObject CharacterGameObject
        {
            get
            {
                return gameObject;
            }
        }

        public int LifePoints
        {
            get
            {
                return _lifePoints;
            }
        }

        public int MaxLifePoints
        {
            get
            {
                return Max_Life_Points;
            }
        }

        public bool InPain
        {
            get { return false; }
        }

        public int Attacked(GameObject attacker, int damage, Vector3? hitPoint, DamageType damageType, AttackTypeDetail attackType = AttackTypeDetail.Generic, int comboNumber = 1, bool showDamaged = false)
        {
            if (!CanBeAttacked)
            {
                return 0;
            }

            _lifePoints--;
            Phase = NextPhase();

            if(OnDamaged != null)
            {
                OnDamaged(this, 1);
            }

            if (_lifePoints == 0)
            {
                Kill();
            }

            return 1;
        }

        public void EndGrappled()
        {
        }

        public void Kill()
        {
            if(OnKilled != null)
            {
                OnKilled(this);
            }
            Destroy(this.gameObject);
        }

        public bool StartGrappled(GameObject grapple)
        {
            return false;
        }

        public void Shoot()
        {
            SpawnProjectile(Vector3.right);
        }

        public void ChargedShot()
        {
            _chargeShotCoroutine = StartCoroutine(ChargedShotRoutine());
        }

        public void TripleShoot()
        {
            SpawnProjectile(new Vector3(1, 1));
            SpawnProjectile(Vector3.right);
            SpawnProjectile(new Vector3(1, -1));
        }

        public void MoveTo(Vector3 target)
        {
            MoveTo(target, Speed);
        }

        public void MoveToRegenerationPosition(Vector3 target)
        {
            MoveTo(target, _regenerationSpeed);
        }

        public void Follow(Transform target)
        {
            if (_movementRoutine == null)
            {
                _movementRoutine = StartCoroutine(FollowRoutine(target));
            }
        }

        public void StopMovement()
        {
            if (_movementRoutine != null)
            {
                StopCoroutine(_movementRoutine);
                _movementRoutine = null;
            }
        }

        public void RegenerateEnergy(bool regenerate)
        {
            CanBeAttacked = regenerate;
            _renderer.sprite = regenerate ? _regeneratingSprite : _defaultSprite;
        }

        private void Awake()
        {
            _renderer.sprite = _defaultSprite;
            _chargedBeam.SetActive(false);
        }

        private Projectile SpawnProjectile(Vector3 direction)
        {
            var position = _projectileSpawnPosition != null ? _projectileSpawnPosition.position : transform.position;
            var projectile = Instantiate(_projectilePrefab, position, Quaternion.identity);
            projectile.Direction = direction;
            projectile.Attacker = gameObject;
            projectile.EnemyLayer = _enemyLayer;
            projectile.Damage = 1;
            return projectile;
        }

        private IEnumerator FollowRoutine(Transform target)
        {
            while (true)
            {
                var charPosition = _character.position;
                var yDirection = target.position.y - charPosition.y;
                var deltaY = charPosition.y +  Speed * Time.deltaTime * Mathf.Sign(yDirection);
                deltaY = Mathf.Clamp(deltaY, _bottom.position.y, _top.position.y);
                charPosition.y = deltaY;
                _character.position = charPosition;
                yield return null;
            }
        }

        private IEnumerator ChargedShotRoutine()
        {
            yield return new WaitForSeconds(_chargeTime);
            StopMovement();
            if(OnChargedShotCharged != null)
            {
                OnChargedShotCharged(this);
            }
            _chargedBeam.SetActive(true);
            yield return new WaitForSeconds(_chargeShootingTime);
            _chargedBeam.SetActive(false);
            _chargeShotCoroutine = null;

            if (OnChargedShotFired != null)
            {
                OnChargedShotFired(this);
            }
        }

        private Phase NextPhase()
        {
            switch (Phase)
            {
                case Phase.Phase1: return Phase.Phase2;
                case Phase.Phase2: return Phase.Phase3;
                default: return Phase.Phase3;
            }
        }

        private void MoveTo(Vector3 target, float speed)
        {
            if (_movementRoutine == null)
            {
                var distance = Vector3.Distance(_character.position, target);
                var moveTo =
                    Common.CoroutineHelpers.MoveTo(
                        _character.position,
                        target,
                        distance / speed,
                        _character,
                        () => _movementRoutine = null);
                _movementRoutine = StartCoroutine(moveTo);
            }
        }
    }
}
