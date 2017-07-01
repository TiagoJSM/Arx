using CommonInterfaces.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts
{
    public class PlayAnimatorOnDamaged : MonoBehaviour, ICharacter
    {
        [SerializeField]
        private Animator _animator;

        public bool CanBeAttacked
        {
            get
            {
                return true;
            }
        }

        public GameObject CharacterGameObject
        {
            get
            {
                return gameObject;
            }
        }

        public bool IsEnemy
        {
            get
            {
                return true;
            }
        }

        public int LifePoints
        {
            get
            {
                return 1;
            }
        }

        public int MaxLifePoints
        {
            get
            {
                return 1;
            }
        }

        public int Attacked(GameObject attacker, int damage, Vector3? hitPoint, DamageType damageType, AttackTypeDetail attackType = AttackTypeDetail.Generic, int comboNumber = 1)
        {
            _animator.enabled = true;
            return 0;
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
    }
}
