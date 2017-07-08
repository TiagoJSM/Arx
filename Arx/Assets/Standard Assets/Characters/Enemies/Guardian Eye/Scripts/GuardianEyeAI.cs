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

        [Header("Phase1")]
        [SerializeField]
        [Range(1, 10)]
        private int _minShotCount = 1;
        [SerializeField]
        [Range(1, 10)]
        private int _maxShotCount = 10;
        [SerializeField]
        [Range(1, 10)]
        private int _minShootWait = 1;
        [SerializeField]
        [Range(1, 10)]
        private int _maxShootWait = 10;
        [SerializeField]
        private Transform _enemy;

        private void Awake()
        {
            _guardianEye = GetComponent<GuardianEye>();
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
            yield return Phase1();
            yield return Phase2();
        }

        private IEnumerator Phase1()
        {
            while (_guardianEye.Phase == Phase.Phase1)
            {
                _movementRoutine = StartCoroutine(MoveToBorders());
                yield return Phase1Shooting();
                yield return ChargedShot();
                StopMovemement();
                yield return MoveTo(_guardianEye.Bottom.position);
                yield return RegenerateEnergy();
            }
        }

        private IEnumerator Phase2()
        {
            while (_guardianEye.Phase == Phase.Phase2)
            {
                _guardianEye.Follow(_enemy);
                yield return Phase1Shooting();
                yield return ChargedShot();
                yield return MoveTo(_guardianEye.Top.position);
                yield return RegenerateEnergy();
            }
        }

        private void StopMovemement()
        {
            StopCoroutine(_movementRoutine);
            _movementRoutine = null;
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
            yield return new WaitUntil(() => _energyRegenerationDuration <= _regenElapsed || _tookDamage);
        }

        private IEnumerator Phase1Shooting()
        {
            var shotCount = UnityEngine.Random.Range(_minShotCount, _maxShotCount);

            while (shotCount > 0)
            {
                var waitTime = UnityEngine.Random.Range(_minShootWait, _maxShootWait);
                yield return new WaitForSeconds(waitTime);
                _guardianEye.Shoot();
                shotCount--;
            }
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
    }
}
