using Assets.Standard_Assets._2D.Scripts.Characters.Enemies;
using Assets.Standard_Assets.Characters.CharacterBehaviour;
using Assets.Standard_Assets.Common;
using Assets.Standard_Assets.Scripts;
using CommonInterfaces.Controllers;
using CommonInterfaces.Enums;
using Extensions;
using GenericComponents.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utils;

namespace Assets.Standard_Assets._2D.Scripts.Controllers
{
    public delegate void OnKilled(BasePlatformerController character);
    public delegate void OnAttacked(BasePlatformerController character, GameObject attacker);

    [RequireComponent(typeof(CharacterStatus))]
    public class BasePlatformerController : MonoBehaviour, ICharacter
    {
        private Direction _direction;

        [SerializeField]
        private bool _takeExtraFromBackAttack = true;
        [SerializeField]
        private float _backAttackDamageMultiplier = 1.5f;
        [SerializeField]
        private StunDamage _stunDamage;
        [SerializeField]
        private AudioSource[] _attackedSounds;

        public bool CanBeAttacked { get; set; }
        public bool InPain { get; set; }

        public event OnKilled OnKilled;
        public event OnAttacked OnAttacked;

        public Direction Direction
        {
            get
            {
                return _direction;
            }
        }

        protected float GroundContactMaxNormal
        {
            get
            {
                return 0.5f;
            }
        }

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
                return CharacterStatus.health.lifePoints;
            }
        }

        public int MaxLifePoints
        {
            get
            {
                return CharacterStatus.health.maxLifePoints;
            }
        }

        public GameObject CharacterGameObject
        {
            get
            {
                return gameObject;
            }
        }

        public CharacterStatus CharacterStatus { get; private set; }
        public bool HitLastTurn { get; private set; }
        public float LastHitDirection { get; private set; }

        public virtual int Attacked(
            GameObject attacker,
            int damage,
            Vector3? hitPoint,
            DamageType damageType,
            AttackTypeDetail attackType = AttackTypeDetail.Generic,
            int comboNumber = 1,
            bool showPain = false)
        {
            if (!CanBeAttacked)
            {
                return 0;
            }
            var damageOriginPosition = hitPoint ?? attacker.transform.position;
            if (_takeExtraFromBackAttack && IsBehind(damageOriginPosition))
            {
                damage = (int)Math.Ceiling(damage * _backAttackDamageMultiplier);
            }
            CharacterStatus.Damage(damage);

            RaiseOnAttacked(attacker);

            if (CharacterStatus.HealthDepleted)
            {
                Kill();
                if (OnKilled != null)
                {
                    OnKilled(this);
                }
            }
            else if(damage > 0)
            {
                var sound = _attackedSounds.Random();
                if(sound != null)
                {
                    sound.Play();
                }
            }

            LastHitDirection = Math.Sign(damageOriginPosition.x - transform.position.x);
            HitLastTurn = true;
            InPain = _stunDamage.DoesStun(damageType, attackType, comboNumber) || showPain;

            return damage;
        }

        public virtual void Kill() { }

        public virtual bool StartGrappled(GameObject grapple)
        {
            this.transform.SetParent(grapple.transform);
            return true;
        }

        public void EndGrappled()
        {
            this.transform.SetParent(null);
        }

        public BasePlatformerController()
        {
            CanBeAttacked = true;
        }

        protected virtual void Awake()
        {
            CharacterStatus = GetComponent<CharacterStatus>();
            _direction = DirectionOfMovement(transform.localScale.x, Direction.Left);
        }

        protected virtual void Update()
        {
            HitLastTurn = false;
        }

        protected void Flip(Direction direction)
        {
            var globalScale = transform.lossyScale;
            if ((direction == Direction.Right && globalScale.x > 0) || (direction == Direction.Left && globalScale.x < 0))
            {
                return;
            }
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            _direction = direction;
        }

        protected Direction DirectionOfMovement(float horizontal, Direction defaultValue)
        {
            return MovementUtils.DirectionOfMovement(horizontal, defaultValue);
        }

        protected float DirectionValue(Direction defaultValue)
        {
            return defaultValue.DirectionValue();
        }

        protected bool IsBehind(Vector3 position)
        {
            var currentPosition = transform.position;
            return
                Direction == Direction.Right
                    ? position.x < currentPosition.x
                    : position.x > currentPosition.x;
        }

        protected void RaiseOnAttacked(GameObject attacker)
        {
            if (OnAttacked != null)
            {
                OnAttacked(this, attacker);
            }
        }
    }
}
