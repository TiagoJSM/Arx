using CommonInterfaces.Controllers;
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

        public BombBehaviour()
        {
            _targets = new Collider2D[10];
        }

        public void Throw(Vector3 force, LayerMask layerMask, GameObject attacker)
        {
            _body.AddForce(force, ForceMode2D.Impulse);
            StartCoroutine(ThrowRoutine(layerMask, attacker));
        }

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
        }

        private IEnumerator ThrowRoutine(LayerMask layerMask, GameObject attacker)
        {
            yield return new WaitForSeconds(_detonationTime);
            var count = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, _targets, layerMask);
            for(var idx = 0; idx < count; idx++)
            {
                var character = _targets[idx].GetComponent<ICharacter>();
                if(character != null)
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
            _explosion.SetActive(true);
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
