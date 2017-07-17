using CommonInterfaces.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.OnDamagedHandlers
{
    public class PlayAnimatorOnDamaged : MonoBehaviour
    {
        [SerializeField]
        private DummyCharacter _dummyCharacter;
        [SerializeField]
        private Animator _animator;

        private void Awake()
        {
            _dummyCharacter.OnAttacked += OnAttackedHandler;
        }

        private void OnAttackedHandler(GameObject attacker, int damage, Vector3? hitPoint, DamageType damageType, AttackTypeDetail attackType, int comboNumber)
        {
            _animator.enabled = true;
        }
    }
}
