using Assets.Standard_Assets.Characters.CharacterBehaviour;
using GenericComponents.Behaviours;
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
        private CharacterStatus _characterStatus;

        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private CanvasGroup _heartsContainer;
        [SerializeField]
        private HeartController _heartPrefab;
        [SerializeField]
        private OverflowLifePointsContainer _overflowLifePoints;
        [SerializeField]
        private int _maxNumberOfHearts;
        [SerializeField]
        private float _delayBetweenHearts = 0.2f;
        [SerializeField]
        private Image _staminaBar;

        public CharacterStatus CharacterStatus
        {
            get
            {
                return _characterStatus;
            }
            set
            {
                _characterStatus = value;
                InstantiateHearts();
                SetDisplayState();
            }
        }

        public CharacterStamina CharacterStamina { get; set; }

        public void Update()
        {
            if(CharacterStatus != null)
            {
                if (_health != CharacterStatus.health.lifePoints)
                {
                    Health = CharacterStatus.health.lifePoints;
                }
            }
            if(CharacterStamina != null)
            {
                var ratio = CharacterStamina.StaminaRatio;
                if(ratio == 0)
                {
                    _staminaBar.enabled = false;
                }
                else
                {
                    _staminaBar.enabled = true;
                    var localScale = _staminaBar.transform.localScale;
                    _staminaBar.transform.localScale = new Vector3(ratio, localScale.y, localScale.z);
                }
                _staminaBar.color = CharacterStamina.IsTired ? Color.red : Color.yellow;
            }
        }

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
            }
        }

        public void InstantiateHearts(bool setHealth = false)
        {
            _heartControllers = new HeartController[_maxNumberOfHearts];
            for (var idx = 0; idx < _maxNumberOfHearts; idx++)
            {
                _heartControllers[idx] = Instantiate(_heartPrefab, _heartsContainer.transform);
            }

            if (setHealth)
            {
                _health = _maxNumberOfHearts * 2;
                _currentHealthCounter = _health;
            }
        }

        private IEnumerator DisplayHeartsRoutine(int oldValue, int newValue)
        {
            var maxLifePoints = _heartControllers.Length * 2;
            var showHearts = newValue <= maxLifePoints;
            _overflowLifePoints.LifePoints = newValue;
            _animator.SetBool("Show Hearts", showHearts);

            newValue = Mathf.Clamp(newValue, 0, maxLifePoints);
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

        private void SetDisplayState()
        {
            _health = _characterStatus.health.lifePoints;

            var maxLifePoints = _heartControllers.Length * 2;

            _currentHealthCounter = Math.Min(_health, maxLifePoints);

            var showHearts = _health <= maxLifePoints;
            var state = showHearts  ? "Show Hearts" : "Hide Hearts";
            _overflowLifePoints.LifePoints = _health;
            _animator.SetBool("Show Hearts", showHearts);
            _animator.PlayInFixedTime(state, -1, 1.0f);

            for (var idx = 0; idx < _heartControllers.Length * 2; idx++)
            {
                var isPair = (idx % 2) == 0;
                var side = isPair ? HeartSide.Left : HeartSide.Right;
                var heartIdx = idx / 2;

                var heart = _heartControllers[heartIdx];
                if (idx < _health)
                {
                    heart.SetFull(side);
                }
                else
                {
                    heart.SetEmpty(side);
                }
            }
        }
    }
}
