using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericComponents.Controllers.Characters;
using UnityEngine;
using CharController = GenericComponents.Controllers.Characters.CharacterController;
using Assets.Standard_Assets.Environment.Hazards.Enemy_Activation_Trigger.Scripts;

namespace Assets.Standard_Assets.Environment.Enemy_Secure_Door.Scripts
{
    public class EnemySecureDoor : MonoBehaviour
    {
        private int _enemiesAlive;

        [SerializeField]
        private CharController[] _characters;
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

        private void OnKilledHandler(CharController character)
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
