using Assets.Standard_Assets.Scripts;
using CommonInterfaces.Controllers;
using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Weapons
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BombBehaviour : MonoBehaviour
    {
        private Rigidbody2D _body;
        private Collider2D[] _targets;
        private LayerMask _enemyLayer;
        private GameObject _attacker;
        private Coroutine _countdownRoutine;

        [SerializeField]
        private GameObject _explosion;
        [SerializeField]
        private float _detonationTime = 3;
        [SerializeField]
        private float _radius = 5;
        [SerializeField]
        private int _maxRangeDamage = 6;
        [SerializeField]
        private int _minRangeDamage = 2;
        [SerializeField]
        private GameObject _visual;

        public BombBehaviour()
        {
            _targets = new Collider2D[10];
        }

        public void Throw(Vector3 force, LayerMask layerMask, GameObject attacker)
        {
            _enemyLayer = layerMask;
            _attacker = attacker;
            _body.AddForce(force, ForceMode2D.Impulse);
            _countdownRoutine = StartCoroutine(ThrowRoutine());
        }

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
        }

        private IEnumerator ThrowRoutine()
        {
            yield return new WaitForSeconds(_detonationTime);
            _countdownRoutine = null;
            yield return ExplosionRoutine(_enemyLayer, _attacker);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var isEnemy = _enemyLayer.IsInAnyLayer(collision.gameObject);
            if(isEnemy)
            {
                if(_countdownRoutine != null)
                {
                    StopCoroutine(_countdownRoutine);
                    StartCoroutine(ExplosionRoutine(_enemyLayer, _attacker));
                    _countdownRoutine = null;
                }
            }
        }

        private void Explode(LayerMask layerMask, GameObject attacker)
        {
            var count = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, _targets, layerMask);
            for (var idx = 0; idx < count; idx++)
            {
                var character = _targets[idx].GetComponent<ICharacter>();
                if (character != null)
                {
                    var distance = Vector2.Distance(character.CharacterGameObject.transform.position, transform.position);
                    character.Attacked(
                        attacker,
                        Mathf.RoundToInt(Mathf.Lerp(_maxRangeDamage, _minRangeDamage, distance / _radius)),
                        character.CharacterGameObject.transform.position,
                        DamageType.Environment,
                        showDamaged: true);
                }

            }
            _visual.SetActive(false);
            _explosion.SetActive(true);
        }

        private IEnumerator ExplosionRoutine(LayerMask layerMask, GameObject attacker)
        {
            Explode(layerMask, attacker);
            yield return new WaitForSeconds(5);
            Destroy(this.gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
