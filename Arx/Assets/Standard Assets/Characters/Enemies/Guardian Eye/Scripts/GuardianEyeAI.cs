using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Enemies.Guardian_Eye.Scripts
{
    [RequireComponent(typeof(GuardianEye))]
    public class GuardianEyeAI : MonoBehaviour
    {
        private GuardianEye _guardianEye;
        private Coroutine _mainRoutine;
        private Coroutine _movementRoutine;
        private bool _chargeShotFired;
        private bool _tookDamage;
        private float _regenElapsed;

        [SerializeField]
        [Range(1, 10)]
        private int _energyRegenerationDuration = 5;
        [SerializeField]
        private Transform _enemy;
        [SerializeField]
        [Range(1, 10)]
        private int _minShotCount = 1;
        [SerializeField]
        [Range(1, 10)]
        private int _maxShotCount = 10;

        [Header("Phase1")]
        [SerializeField]
        [Range(0.1f, 2)]
        private float _minShootWaitPhase1 = 0.1f;
        [SerializeField]
        [Range(0.1f, 2)]
        private float _maxShootWaitPhase1 = 2;

        [Header("Phase2")]
        [SerializeField]
        [Range(0.1f, 2)]
        private float _minShootWaitPhase2 = 0.1f;
        [SerializeField]
        [Range(0.1f, 2)]
        private float _maxShootWaitPhase2 = 2;

        [Header("Phase3")]
        [SerializeField]
        [Range(0.1f, 2)]
        private float _minShootWaitPhase3 = 0.1f;
        [SerializeField]
        [Range(0.1f, 2)]
        private float _maxShootWaitPhase3 = 2;

        private void Awake()
        {
            _guardianEye = GetComponent<GuardianEye>();
            _guardianEye.OnChargedShotCharged += OnChargedShotChargedHandler;
            _guardianEye.OnChargedShotFired += OnChargedShotFiredHandler;
            _guardianEye.OnDamaged += OnDamagedHandler;
        }

        private void Start()
        {
            _mainRoutine = StartCoroutine(GuardianEyeRoutine());
        }

        private void Update()
        {
            _regenElapsed += Time.deltaTime;
        }

        private void OnChargedShotChargedHandler(GuardianEye character)
        {
            StopMovemement();
        }

        private void OnChargedShotFiredHandler(GuardianEye character)
        {
            _chargeShotFired = true;
        }

        private void OnDamagedHandler(GuardianEye character, int damage)
        {
            _tookDamage = true;
        }

        private IEnumerator GuardianEyeRoutine()
        {
            yield return PhaseAction(Phase.Phase1, _guardianEye.Bottom.position, () => _movementRoutine = StartCoroutine(MoveToBorders()));
            yield return PhaseAction(Phase.Phase2, _guardianEye.Bottom.position, () => _guardianEye.Follow(_enemy));
            yield return PhaseAction(Phase.Phase3, _guardianEye.Bottom.position, () => _guardianEye.Follow(_enemy));
        }

        private IEnumerator PhaseAction(Phase currentPhase, Vector3 restPosition, Action movement)
        {
            while (_guardianEye.Phase == currentPhase)
            {
                movement();
                yield return Shooting();
                yield return ChargedShot();
                yield return MoveToRegenerationPosition(restPosition);
                yield return RegenerateEnergy();
            }
        }

        private void StopMovemement()
        {
            if(_movementRoutine != null)
            {
                StopCoroutine(_movementRoutine);
                _movementRoutine = null;
            }
            _guardianEye.StopMovement();
        }

        private IEnumerator ChargedShot()
        {
            _chargeShotFired = false;
            _guardianEye.ChargedShot();
            yield return new WaitUntil(() => _chargeShotFired);
        }

        private IEnumerator RegenerateEnergy()
        {
            _regenElapsed = 0;
            _tookDamage = false;
            _guardianEye.RegenerateEnergy(true);
            yield return new WaitUntil(() => _energyRegenerationDuration <= _regenElapsed || _tookDamage);
            _guardianEye.RegenerateEnergy(false);
        }

        private IEnumerator Shooting()
        {
            var shotCount = UnityEngine.Random.Range(_minShotCount, _maxShotCount);

            while (shotCount > 0)
            {
                yield return ShootingWaitTime();
                if(_guardianEye.Phase == Phase.Phase3)
                {
                    _guardianEye.TripleShoot();
                }
                else
                {
                    _guardianEye.Shoot();
                }
                shotCount--;
            }
        }

        private IEnumerator ShootingWaitTime()
        {
            float minWait, maxWait;
            switch (_guardianEye.Phase)
            {
                case Phase.Phase1:
                    minWait = _minShootWaitPhase1; maxWait = _maxShootWaitPhase1; break;
                case Phase.Phase2:
                    minWait = _minShootWaitPhase2; maxWait = _maxShootWaitPhase2; break;
                case Phase.Phase3:
                default:
                    minWait = _minShootWaitPhase3; maxWait = _maxShootWaitPhase3; break;
            }

            var waitTime = UnityEngine.Random.Range(minWait, maxWait);
            yield return new WaitForSeconds(waitTime);
        }

        private IEnumerator MoveToBorders()
        {
            while (true)
            {
                yield return MoveTo(_guardianEye.Top.position);
                yield return MoveTo(_guardianEye.Bottom.position);
            }
        }

        private IEnumerator MoveTo(Vector3 target)
        {
            _guardianEye.MoveTo(target);
            yield return new WaitWhile(() => _guardianEye.IsMoving);
        }

        private IEnumerator MoveToRegenerationPosition(Vector3 target)
        {
            _guardianEye.MoveTo(target);
            yield return new WaitWhile(() => _guardianEye.IsMoving);
        }
    }
}
