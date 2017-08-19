using CommonInterfaces.Controllers;
using GenericComponents.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Controllers.Characters
{
    public delegate void OnKilled(CharacterController character);

    [RequireComponent(typeof(CharacterStatus))]
    public class CharacterController : MonoBehaviour, ICharacter
    {
        private CharacterStatus _status;

        public bool CanBeAttacked { get; protected set; }
        public event OnKilled OnKilled;

        public virtual bool IsEnemy
        {
            get
            {
                return false;
            }
        }

        public int LifePoints
        {
            get
            {
                return _status.health.lifePoints;
            }
        }

        public int MaxLifePoints
        {
            get
            {
                return _status.health.maxLifePoints;
            }
        }

        public GameObject CharacterGameObject
        {
            get
            {
                return gameObject;
            }
        }

        public virtual int Attacked(
            GameObject attacker, 
            int damage, 
            Vector3? hitPoint,
            DamageType damageType,
            AttackTypeDetail attackType = AttackTypeDetail.Generic,
            int comboNumber = 1)
        {
            if (!CanBeAttacked)
            {
                return 0;
            }
            _status.Damage(damage);
            if (_status.HealthDepleted)
            {
                Kill();
                if(OnKilled != null)
                {
                    OnKilled(this);
                }
            }
            return LifePoints;
        }

        public virtual void Kill()
        {
            Destroy(this.gameObject);
        }

        public virtual bool StartGrappled(GameObject grapple)
        {
            this.transform.SetParent(grapple.transform);
            return true;
        }

        public void EndGrappled()
        {
            this.transform.SetParent(null);
        }

        public CharacterController()
        {
            CanBeAttacked = true;
        }

        protected virtual void Awake()
        {
            _status = GetComponent<CharacterStatus>();
        }
    }
}
