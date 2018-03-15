using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.CharacterBehaviour
{
    public enum StaminaStatus
    {
        Iddle,
        Regenerating,
        Consuming
    }

    public class CharacterStamina : MonoBehaviour
    {
        private float _currentStamina;
        private Coroutine _staminaCoroutine;
        private StaminaStatus _staminaStatus;

        [SerializeField]
        private float _stamina = 10;
        [SerializeField]
        private float _staminaRegenerationDelay = 3;
        [SerializeField]
        private float _staminaTiredValue = 2f;

        public event Action StaminaDepleted;

        public bool IsTired { get { return _currentStamina < _staminaTiredValue; } }
        public float StaminaRatio { get { return _currentStamina / _stamina; } }

        private void Awake()
        {
            _currentStamina = _stamina;
        }

        public void ConsumeAllStamina()
        {
            _currentStamina = 0;
            ActivateCoroutine(RegenerateStaminaRoutine());
        }

        public void ConsumeStamina()
        {
            if (_staminaStatus != StaminaStatus.Consuming)
            {
                ActivateCoroutine(ConsumeStaminaRoutine());
            }
        }

        public void ConsumeStaminaInstant(float value)
        {
            _currentStamina -= value;
            if (_currentStamina < 0)
            {
                _currentStamina = 0;
            }
            ActivateCoroutine(RegenerateStaminaRoutine());
        }

        public void RegenerateStamina()
        {
            if (_staminaStatus != StaminaStatus.Regenerating)
            {
                ActivateCoroutine(RegenerateStaminaRoutine());
            }
        }

        private void ActivateCoroutine(IEnumerator routine)
        {
            if (_staminaCoroutine != null)
            {
                StopCoroutine(_staminaCoroutine);
            }
            _staminaCoroutine = StartCoroutine(routine);
        }

        private IEnumerator ConsumeStaminaRoutine()
        {
            _staminaStatus = StaminaStatus.Consuming;
            while (_currentStamina > 0)
            {
                yield return null;
                _currentStamina -= Time.deltaTime;
            }

            if (_currentStamina < 0)
            {
                _currentStamina = 0;
            }

            if(StaminaDepleted != null)
            {
                StaminaDepleted();
            }

            _staminaCoroutine = StartCoroutine(RegenerateStaminaRoutine());
        }

        private IEnumerator RegenerateStaminaRoutine()
        {
            _staminaStatus = StaminaStatus.Regenerating;
            yield return new WaitForSeconds(_staminaRegenerationDelay);

            while (_currentStamina < _stamina)
            {
                yield return null;
                _currentStamina += Time.deltaTime;
            }

            if (_currentStamina > _stamina)
            {
                _currentStamina = _stamina;
            }
            _staminaCoroutine = null;
            _staminaStatus = StaminaStatus.Iddle;
        }
    }
}
