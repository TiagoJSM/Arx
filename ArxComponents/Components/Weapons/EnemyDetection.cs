using ArxGame.Components.EnemyControllers;
using CommonInterfaces.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components.Weapons
{
    public class EnemyDetection : MonoBehaviour
    {
        private List<BaseEnemyController> _attackedEnemies;

        public ICharacter Owner { get; set; }

        public void AttackIsOver()
        {
            _attackedEnemies.Clear();
        }

        void Awake()
        {
            _attackedEnemies = new List<BaseEnemyController>();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            var enemy = other.GetComponent<BaseEnemyController>();
            if (_attackedEnemies.Contains(enemy))
            {
                return;
            }
            _attackedEnemies.Add(enemy);
            Owner.Attack(enemy, null);
        }
    }
}
