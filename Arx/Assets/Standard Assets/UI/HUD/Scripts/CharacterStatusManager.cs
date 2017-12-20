using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Standard_Assets.UI.HUD.Scripts
{
    public class CharacterStatusManager : MonoBehaviour
    {
        private int _health;
        private int _currentHealthCounter;
        private HeartController[] _heartControllers;
        private Coroutine _displayHearts;

        [SerializeField]
        private HeartController _heartPrefab;
        [SerializeField]
        private int _maxNumberOfHearts;
        [SerializeField]
        private float _delayBetweenHearts = 0.2f;

        public void Awake()
        {
            InstantiateHearts();
        }

        public int MaxHealth { get; set; }
        public int Health
        {
            get
            {
                return _health;
            }
            set
            {
                if (_health != value)
                {
                    if(_displayHearts != null)
                    {
                        StopCoroutine(_displayHearts);
                    }
                    _displayHearts = StartCoroutine(DisplayHeartsRoutine(_currentHealthCounter, value));
                    _health = value;
                }
                //Mathf.Clamp(value, 0, MaxHealth);

            }
        }

        public void InstantiateHearts(bool setHealth = false)
        {
            _heartControllers = new HeartController[_maxNumberOfHearts];
            for (var idx = 0; idx < _maxNumberOfHearts; idx++)
            {
                _heartControllers[idx] = Instantiate(_heartPrefab, gameObject.transform);
            }

            if (setHealth)
            {
                _health = _maxNumberOfHearts * 2;
                _currentHealthCounter = _health;
            }
        }

        private IEnumerator DisplayHeartsRoutine(int oldValue, int newValue)
        {
            if(newValue > oldValue)
            {
                return GainHearts(oldValue, newValue);
            }
            else
            {
                return LoseHearts(oldValue, newValue);
            }
        }

        private IEnumerator GainHearts(int oldValue, int newValue)
        {
            oldValue++;
            for (; newValue >= oldValue; oldValue++)
            {
                var isPair = (oldValue % 2) == 0;
                var idx = oldValue / 2 - (isPair ? 1 : 0);

                var heart = _heartControllers[idx];

                var side = isPair ? HeartSide.Right : HeartSide.Left;

                heart.Gain(side);
                _currentHealthCounter = oldValue;

                yield return new WaitForSeconds(_delayBetweenHearts);
            }

            _displayHearts = null;
        }

        private IEnumerator LoseHearts(int oldValue, int newValue)
        {
            for (; oldValue > newValue; oldValue--)
            {
                var isPair = (oldValue % 2) == 0;
                var idx = oldValue / 2 - (isPair ? 1 : 0);

                var heart = _heartControllers[idx];

                var side = isPair ? HeartSide.Right : HeartSide.Left;

                heart.Lose(side);
                _currentHealthCounter = oldValue - 1;

                yield return new WaitForSeconds(_delayBetweenHearts);
            }

            _displayHearts = null;
        }
    }
}
