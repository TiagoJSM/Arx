using Assets.Standard_Assets.Characters.CharacterBehaviour;
using Assets.Standard_Assets.Effects.DamageFlash.Scripts;
using Assets.Standard_Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Hazards.Blocking_Wood.Scripts
{
    [RequireComponent(typeof(CharacterStatus))]
    [RequireComponent(typeof(DefaultColorFlash))]
    public class BlockingWood : MonoBehaviour, ICharacter
    {
        private CharacterStatus _characterStatus;
        private DefaultColorFlash _flash;

        public bool CanBeAttacked { get { return false; } }
        public GameObject CharacterGameObject { get { return gameObject; } }
        public bool InPain { get { return false; } }
        public int LifePoints { get { return _characterStatus.health.lifePoints; } }
        public int MaxLifePoints { get { return _characterStatus.health.maxLifePoints; } }

        public int Attacked(GameObject attacker, int damage, Vector3? hitPoint, DamageType damageType, AttackTypeDetail attackType = AttackTypeDetail.Generic, int comboNumber = 1, bool showDamaged = false)
        {
            _characterStatus.Damage(damage);
            if (_characterStatus.HealthDepleted)
            {
                Destroy(gameObject);
            }
            else
            {
                _flash.Flash();
            }
            return damage;
        }

        public void EndGrappled()
        {
            
        }

        public void Kill()
        {
            
        }

        public bool StartGrappled(GameObject grapple)
        {
            return false;
        }

        private void Awake()
        {
            _characterStatus = GetComponent<CharacterStatus>();
            _flash = GetComponent<DefaultColorFlash>();
        }
    }
}
