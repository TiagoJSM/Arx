using CommonInterfaces.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components.EnemyControllers
{
    [RequireComponent(typeof(CharacterStatus))]
    public abstract class BaseEnemyController : MonoBehaviour, ICharacter
    {
        public bool CanBeAttacked { get; protected set; }
        
        public bool IsEnemy
        {
            get
            {
                return true;
            }
        }

        public int MaxLifePoints
        {
            get
            {
                return CharacterStatus.health.maxLifePoints;
            }
        }

        public int LifePoints
        {
            get
            {
                return CharacterStatus.health.lifePoints;
            }
        }

        protected CharacterStatus CharacterStatus { get; private set; }

        public void Kill()
        {
            Destroy(this.gameObject);
        }

        void Awake()
        {
            CharacterStatus = GetComponent<CharacterStatus>();
            CanBeAttacked = true;
            OnAwake();
        }

        public abstract bool Attacked(GameObject attacker, int damage, Vector3? hitPoint);

        protected virtual void OnAwake() { }

    }
}
