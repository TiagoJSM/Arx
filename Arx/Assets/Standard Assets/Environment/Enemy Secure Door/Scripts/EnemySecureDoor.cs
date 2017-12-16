using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericComponents.Controllers.Characters;
using UnityEngine;
using Assets.Standard_Assets.Environment.Hazards.Enemy_Activation_Trigger.Scripts;
using Assets.Standard_Assets._2D.Scripts.Controllers;

namespace Assets.Standard_Assets.Environment.Enemy_Secure_Door.Scripts
{
    public class EnemySecureDoor : MonoBehaviour
    {
        private int _enemiesAlive;

        [SerializeField]
        private BasePlatformerController[] _characters;
        [SerializeField]
        private EnemyActivationTrigger _enemyActivationTrigger;

        private void Start()
        {
            _enemyActivationTrigger.OnActivate += OnActivateHandler;
            _enemiesAlive = _characters.Length;
            for(var idx = 0; idx < _characters.Length; idx++)
            {
                _characters[idx].OnKilled += OnKilledHandler;
            }
            this.gameObject.SetActive(false);
        }

        private void OnActivateHandler(EnemyActivationTrigger trigger)
        {
            this.gameObject.SetActive(true);
            _enemyActivationTrigger.OnActivate -= OnActivateHandler;
        }

        private void OnKilledHandler(BasePlatformerController character)
        {
            character.OnKilled -= OnKilledHandler;
            _enemiesAlive--;
            if(_enemiesAlive == 0)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
