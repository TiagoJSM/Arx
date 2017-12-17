using Assets.Standard_Assets._2D.Scripts.Characters;
using Assets.Standard_Assets._2D.Scripts.Characters.CommonAiBehaviour;
using Assets.Standard_Assets.Common;
using GenericComponents.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GenericComponents.Controllers.Characters;
using Assets.Standard_Assets.Extensions;
using Assets.Standard_Assets._2D.Scripts.Controllers;
using System.Collections;

namespace Assets.Standard_Assets.Characters.Enemies.Spider_Mine.Scripts
{
    public class SpiderMineAiStateManager : StateManager<SpiderMineAi, object>
    {
        public SpiderMineAiStateManager(SpiderMineAi controller) : base(controller)
        {
            this.SetInitialState<WaitState>()
                .To<FollowState>((c, a, t) => c.Enemy != null && c.WokeUp);

            this.From<FollowState>()
                .To<BlowUpCountdownState>((c, a, t) => c.IsCloseStillEnemy);

            this.From<BlowUpCountdownState>()
                .To<BlowUpState>((c, a, t) => c.CountdownOver);
        }
    }

    [RequireComponent(typeof(CharacterController2D))]
    [RequireComponent(typeof(CharacterFinder))]
    public class SpiderMineAi : BaseCharacterAiController
    {
        private SpiderMineAiStateManager _aiManager;
        private CharacterController2D _characterController;
        private CharacterFinder _charFinder;

        [SerializeField]
        private BoxCollider2D _collider;
        [SerializeField]
        private float _attackRange = 3f;
        [SerializeField]
        private GameObject _explosion;
        [SerializeField]
        private float _explosionRadius = 20.0f;
        [SerializeField]
        private int _explosionDamage = 2;
        [SerializeField]
        private LayerMask _enemiesLayer;
        [SerializeField]
        private SpriteRenderer _damageArea;
        [SerializeField]
        private SpriteRenderer _damageAreaSurround;
        [SerializeField]
        private float _countdownTime = 3.05f;  //taken from animation

        public GameObject Enemy { get; private set; }
        public bool IsCloseStillEnemy { get; private set; }
        public bool CountdownOver { get; set; }
        public bool WokeUp { get; set; }
        public bool BlowUpCountdown { get; private set; }

        public bool IsTargetInRange
        {
            get
            {
                var currentPosition = this.transform.position;
                var distance = Vector2.Distance(currentPosition, Enemy.transform.position);
                return distance < _attackRange;
            }
        }

        public SpiderMineAi()
        {
            _aiManager = new SpiderMineAiStateManager(this);
        }

        public void FollowEnemy()
        {
            SetActiveCoroutine(
                CoroutineHelpers.FollowTargetCoroutine(
                    transform,
                    Enemy,
                    (movemement) =>
                        _characterController.move(new Vector3(Mathf.Sign(movemement) * 3, -10, 0) * Time.deltaTime),
                    () => IsTargetInRange));
        }

        public void WaitForEnemy()
        {
        }

        public void BlowUp()
        {
            _explosion.SetActive(true);

            var colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _enemiesLayer);
            for(var idx = 0; idx < colliders.Length; idx++)
            {
                if (colliders[idx].IsPlayer())
                {
                    var character = colliders[idx].GetComponent<BasePlatformerController>();
                    character.Attacked(gameObject, _explosionDamage, null, CommonInterfaces.Controllers.DamageType.BodyAttack);
                }
            }
        }

        public void StartCountdown()
        {
            BlowUpCountdown = true;
            StopActiveCoroutine();
            StartCoroutine(DisplayDamageAreaRoutine());
        }

        private IEnumerator DisplayDamageAreaRoutine()
        {
            _damageArea.gameObject.SetActive(true);
            _damageAreaSurround.gameObject.SetActive(true);
            var bounds = _damageArea.bounds;
            var radiusTransform = _damageArea.transform;
            var originalScale = _damageArea.transform.localScale;

            var elapsed = 0.0f;
            while(elapsed < _countdownTime)
            {
                var currentSize = Mathf.Lerp(0.1f, _explosionRadius, elapsed / _countdownTime);

                radiusTransform.localScale = new Vector3(currentSize / bounds.extents.x, currentSize / bounds.extents.y, originalScale.z);

                elapsed += Time.deltaTime;
                yield return null;
            }
            _damageArea.gameObject.SetActive(false);
            _damageAreaSurround.gameObject.SetActive(false);
        }

        private void Start()
        {
            _characterController = GetComponent<CharacterController2D>();
            _charFinder = GetComponent<CharacterFinder>();
            _characterController.BoxCollider2D = _collider;
            _charFinder.OnCharacterFound += OnCharacterFoundHandler;
        }

        private void Update()
        {
            _aiManager.Perform(null);
        }

        private void OnCharacterFoundHandler(BasePlatformerController controller)
        {
            Enemy = controller.gameObject;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IsCloseStillEnemy = collision.gameObject.IsPlayer();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.IsPlayer())
            {
                IsCloseStillEnemy = false;
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }
    }
}
