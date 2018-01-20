using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonInterfaces.Enums;
using UnityEngine;
using Assets.Standard_Assets._2D.Scripts.Controllers;
using GenericComponents.StateMachine;
using Assets.Standard_Assets._2D.Scripts.Characters;
using Assets.Standard_Assets.Characters.CharacterBehaviour;

namespace Assets.Standard_Assets.Characters.Enemies.Alarm_Bot.Scripts
{

    [RequireComponent(typeof(PlatformerCharacterController))]
    [RequireComponent(typeof(AlarmBotSounds))]
    public class AlarmBotAiController : PlatformerCharacterAiControl
    {
        private PlatformerCharacterController _controller;
        private int _aliveEnemyCount;
        private BasePlatformerController _enemy;
        private CharacterAwarenessNotifier _awarenessNotifier;
        private AlarmBotSounds _sounds;

        [SerializeField]
        private CharacterFinder _charFinder;
        [SerializeField]
        private Transform[] _spawnPoints;
        [SerializeField]
        private BasePlatformerController _enemyPrefab;
        [SerializeField]
        private Collider2D _collider;

        public bool Warning { get; private set; }
        public bool Dead { get { return _controller.CharacterStatus.HealthDepleted; } }

        protected override Direction CurrentDirection
        {
            get
            {
                return _controller.Direction;
            }
        }

        protected override Vector2 Velocity
        {
            get
            {
                return _controller.Velocity;
            }
        }

        public override void Move(float directionValue)
        {
            _controller.DoMove(directionValue);
        }

        protected override void Awake()
        {
            _controller = GetComponent<PlatformerCharacterController>();
            _awarenessNotifier = GetComponent<CharacterAwarenessNotifier>();
            _sounds = GetComponent<AlarmBotSounds>();
            _controller.MovementType = MovementType.Walk;
            _controller.OnKilled += OnKilledHandler;
            Warning = false;
            _charFinder.OnCharacterFound += OnCharacterFoundHandler;
            IddleMovement();
            _sounds.IddleMovement();
        }

        private void OnDestroy()
        {
            RemoveEventHandlers();
        }

        private void OnKilledHandler(BasePlatformerController character)
        {
            StopActiveCoroutine();
            _controller.StayStill();
            _collider.enabled = false;
            _sounds.Death();
            RemoveEventHandlers();
        }

        private void OnCharacterFoundHandler(BasePlatformerController controller)
        {
            if(_enemy == null)
            {
                _enemy = controller;
                Warning = true;
                _controller.MovementType = MovementType.Run;
                _sounds.AlarmMovement();
                SpawnEnemies();
                _awarenessNotifier.Notify(controller.gameObject);
            }
        }

        private void SpawnEnemies()
        {
            _aliveEnemyCount = _spawnPoints.Length;

            for(var idx = 0; idx < _aliveEnemyCount; idx++)
            {
                var enemy = Instantiate(_enemyPrefab, _spawnPoints[idx].position, Quaternion.identity);
                enemy.OnKilled += OnEnemyKilled;
            }
        }

        private void OnEnemyKilled(BasePlatformerController character)
        {
            _aliveEnemyCount--;
        }

        private void RemoveEventHandlers()
        {
            _charFinder.OnCharacterFound -= OnCharacterFoundHandler;
            _controller.OnKilled -= OnKilledHandler;
        }
    }
}
