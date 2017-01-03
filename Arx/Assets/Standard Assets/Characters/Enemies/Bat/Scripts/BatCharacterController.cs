using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Enemies.Bat.Scripts
{
    public class BatCharacterController : PlatformerCharacterController
    {
        private Vector2 _movementDirection;
        private bool _attacking;
        private Coroutine _attackCoroutine;

        [SerializeField]
        private float _attackThreshold = 1;
        [SerializeField]
        private int _damage = 1;
        [SerializeField]
        private Vector2 _attackVelocity = Vector2.one;
        [SerializeField]
        private Vector2 _movementVelocity = Vector2.one;

        public Vector2 MovementDirection
        {
            get
            {
                return _movementDirection;
            }
            set
            {
                _movementDirection = value;
                var dir = base.DirectionOfMovement(_movementDirection.x, Direction);
                Flip(dir);
                Velocity = new Vector2(
                    _movementVelocity.x * _movementDirection.x, 
                    _movementVelocity.y * _movementDirection.y);
            }
        }

        public bool Attacking
        {
            get
            {
                return _attacking;
            }
        }

        public void Attack(GameObject character)
        {
            _attacking = true;
            _attackCoroutine = StartCoroutine(AttackCoroutine(character));
        }

        protected override void Awake()
        {
            base.Awake();
            Velocity = _movementVelocity;
            CharacterController2D.onTriggerEnterEvent += OnTriggerEnterEventHandler;
        }

        private void OnTriggerEnterEventHandler(Collider2D collider)
        {
            if (!_attacking)
            {
                return;
            }
            var character = collider.gameObject.GetComponent<CommonInterfaces.Controllers.ICharacter>();
            if (character != null)
            {
                character.Attacked(gameObject, _damage, transform.position);
            }
        }

        private IEnumerator AttackCoroutine(GameObject character)
        {
            var targetPosition = character.transform.position;
            var direction = (character.transform.position - transform.position).normalized;
            MovementDirection = direction;
            Velocity = new Vector2(
                    _attackVelocity.x * _movementDirection.x,
                    _attackVelocity.y * _movementDirection.y);

            while (true)
            {
                var distance = Vector2.Distance(transform.position, targetPosition);
                if(distance <= _attackThreshold)
                {
                    _attacking = false;
                    _attackCoroutine = null;
                    yield break;
                }
                yield return null;
            }
        }
    }
}
