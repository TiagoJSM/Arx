using Assets.Standard_Assets.Scripts;
using CommonInterfaces.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Combat
{
    public class SprintCombatBehaviour : BaseCombatBehaviour
    {
        private List<ICharacter> _charactersAttacked;
        private Coroutine _attackDetector;

        [SerializeField]
        private Transform _stingDashAreaP1;
        [SerializeField]
        private Transform _stingDashAreaP2;
        [SerializeField]
        private Transform _lowKickAttackAreaP1;
        [SerializeField]
        private Transform _lowKickAttackAreaP2;
        [SerializeField]
        private LayerMask _enemyLayer;

        public SprintCombatBehaviour()
        {
            _charactersAttacked = new List<ICharacter>();
        }

        public void StartStingDash()
        {
            _attackDetector = StartCoroutine(AttackDetector(_stingDashAreaP1, _stingDashAreaP2));
        }

        public void EndStingDash()
        {
            if (_attackDetector != null)
            {
                StopCoroutine(_attackDetector);
                _attackDetector = null;
            }
        }

        public void StartLowKick()
        {
            _attackDetector = StartCoroutine(AttackDetector(_lowKickAttackAreaP1, _lowKickAttackAreaP2));
        }

        public void EndLowKick()
        {
            if (_attackDetector != null)
            {
                StopCoroutine(_attackDetector);
                _attackDetector = null;
            }
        }

        private IEnumerator AttackDetector(Transform attackAreaP1, Transform attackAreaP2)
        {
            _charactersAttacked.Clear();
            while (true)
            {
                var enemiesInRange = DetectNewEnemies(
                    _charactersAttacked,
                    attackAreaP1.position,
                    attackAreaP2.position,
                    _enemyLayer);

                if (enemiesInRange.Count() > 0)
                {
                    for(var idx = 0; idx < enemiesInRange.Count; idx++)
                    {
                        var enemy = enemiesInRange[idx];
                        enemy.Attacked(gameObject, 1, transform.position, DamageType.BodyAttack, AttackTypeDetail.Generic, 1, true);
                    }
                }
                yield return null;
            }
        }
    }
}
