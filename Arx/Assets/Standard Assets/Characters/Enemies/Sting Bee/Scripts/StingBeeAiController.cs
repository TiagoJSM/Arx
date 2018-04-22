using Assets.Standard_Assets._2D.Scripts.Characters;
using Assets.Standard_Assets._2D.Scripts.Characters.CommonAiBehaviour;
using Assets.Standard_Assets._2D.Scripts.Controllers;
using Assets.Standard_Assets.Characters.CharacterBehaviour;
using Assets.Standard_Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Enemies.Sting_Bee.Scripts
{
    [RequireComponent(typeof(CharacterController2D))]
    [RequireComponent(typeof(StingDashController))]
    [RequireComponent(typeof(FlyingCharacterIddle))]
    [RequireComponent(typeof(StingThrow))]
    [RequireComponent(typeof(CharacterStatus))]
    [RequireComponent(typeof(RunAwayFromTarget))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(FlyBackToOrigin))]
    [RequireComponent(typeof(DefensiveFlightAroundTarget))]
    public class StingBeeAiController : MonoBehaviour, ICharacter
    {
        private readonly int StingShoot = Animator.StringToHash("Sting Shoot");

        private CharacterController2D _character;
        private StingDashController _stingDash;
        private FlyingCharacterIddle _iddle;
        private StingThrow _stingThrow;
        private CharacterStatus _status;
        private RunAwayFromTarget _runAway;
        private Animator _animator;
        private FlyBackToOrigin _flyBackToOrigin;
        private DefensiveFlightAroundTarget _flyAround;
        private BasePlatformerController _target;
        private Coroutine _aiRoutine;

        [SerializeField]
        private CharacterFinder _characterFinder;
        [SerializeField]
        private BoxCollider2D _collider;
        [SerializeField]
        private float _dashMaxDistance = 4.0f;
        [SerializeField]
        private float _attackDelay = 1.0f;
        [SerializeField]
        private float _targetMaxDistanceFromOrigin = 20.0f;

        public bool CanBeAttacked { get; private set; }
        public GameObject CharacterGameObject { get { return gameObject; } }
        public bool InPain { get { return false; } }
        public int LifePoints { get { return _status.health.lifePoints; } }
        public int MaxLifePoints { get { return _status.health.maxLifePoints; } }
        public bool IsTargetTooFar
        {
            get
            {
                if(_target != null)
                {
                    return Vector2.Distance(_target.transform.position, _flyBackToOrigin.Origin) > _targetMaxDistanceFromOrigin;
                }
                return false;
            }
        }

        public int Attacked(GameObject attacker, int damage, Vector3? hitPoint, DamageType damageType, AttackTypeDetail attackType = AttackTypeDetail.Generic, int comboNumber = 1, bool showDamaged = false)
        {
            if (!CanBeAttacked)
            {
                return 0;
            }
            _status.Damage(damage);
            if (_status.HealthDepleted)
            {
                Destroy(gameObject);
                return damage;
            }
            CanBeAttacked = false;
            _target = attacker.GetComponent<BasePlatformerController>();
            StopAllCoroutines();
            StartCoroutine(_runAway.RunAway(attacker, OnRanAwayFromTarget));
            
            return damage;
        }

        public void EndGrappled()
        {

        }

        public void Kill()
        {
            Destroy(gameObject);
        }

        public bool StartGrappled(GameObject grapple)
        {
            return false;
        }

        private void Awake()
        {
            _character = GetComponent<CharacterController2D>();
            _stingDash = GetComponent<StingDashController>();
            _iddle = GetComponent<FlyingCharacterIddle>();
            _stingThrow = GetComponent<StingThrow>();
            _status = GetComponent<CharacterStatus>();
            _runAway = GetComponent<RunAwayFromTarget>();
            _animator = GetComponent<Animator>();
            _flyBackToOrigin = GetComponent<FlyBackToOrigin>();
            _flyAround = GetComponent<DefensiveFlightAroundTarget>();

            CanBeAttacked = true;
        }

        private void Start()
        {
            _characterFinder.OnCharacterFound += OnCharacterFoundHandler;
            _character.BoxCollider2D = _collider;

            _aiRoutine = StartCoroutine(_iddle.IddleMovement());
        }

        private void OnCharacterFoundHandler(BasePlatformerController controller)
        {
            if(_target == null)
            {
                _target = controller;
                StopAllCoroutines();
                _aiRoutine = StartCoroutine(AiRoutine());
            }
        }

        private IEnumerator AiRoutine()
        {
            while (true)
            {
                yield return _flyAround.FlyAround();
                var action = ((int)Time.time % 2) == 0;
                if (action)
                {
                    yield return _stingDash.StingDash(_target.gameObject);
                }
                else
                {
                    _animator.SetTrigger(StingShoot);
                    yield return _stingThrow.StartStingThrow(_target.gameObject);
                }

                if (IsTargetTooFar)
                {
                    CanBeAttacked = false;
                    yield return _flyBackToOrigin.GoBackToOrigin();
                    _target = null;
                    _aiRoutine = StartCoroutine(_iddle.IddleMovement());
                    CanBeAttacked = true;
                    yield break;
                }
            }
        }

        private void OnRanAwayFromTarget()
        {
            CanBeAttacked = true;
            StopAllCoroutines();
            _aiRoutine = StartCoroutine(AiRoutine());
        }
    }
}
