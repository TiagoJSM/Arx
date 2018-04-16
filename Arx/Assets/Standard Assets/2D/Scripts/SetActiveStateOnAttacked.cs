using Assets.Standard_Assets.Scripts;
using CommonInterfaces.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts
{
    public class SetActiveStateOnAttacked : MonoBehaviour
    {
        [SerializeField]
        private bool _active;
        [SerializeField]
        private DummyCharacter _dummyCharacter;
        [SerializeField]
        private GameObject _gameObject;

        private void Awake()
        {
            _dummyCharacter.OnAttacked += OnAttackedHandler;
        }

        private void OnAttackedHandler(GameObject attacker, int damage, Vector3? hitPoint, DamageType damageType, AttackTypeDetail attackType, int comboNumber)
        {
            _gameObject.SetActive(_active);
        }
    }
}
