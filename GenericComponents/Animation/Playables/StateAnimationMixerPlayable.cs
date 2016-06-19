using GenericComponents.Interfaces;
using GenericComponents.Interfaces.States;
using GenericComponents.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.Director;

namespace GenericComponents.Animation.Playables
{
    public class StateAnimationMixerPlayable<TController, TAction> : AnimationMixerPlayable
    {
        private const float TransitionTime = 0.3f;
        private const float TransitionStartFirstWeight = 0.99f;
        private const float TransitionStartSecondWeight = 0.01f;

        private IState<TController, TAction> _currentState;
        private StateManager<TController, TAction> _stateManager;
        private Dictionary<Type, AnimationPlayable> _animationPlayablesByState;

        private AnimationPlayable _currentAnimation;

        private IState<TController, TAction> State
        {
            get
            {
                if (_currentState == null)
                {
                    _currentState = _stateManager.CurrentState;
                }
                if (_currentState != _stateManager.CurrentState)
                {
                    _currentState = _stateManager.CurrentState;
                }
                return _currentState;
            }
        }

        public StateAnimationMixerPlayable(StateManager<TController, TAction> stateManager)
        {
            _animationPlayablesByState = new Dictionary<Type, AnimationPlayable>();
            _stateManager = stateManager;
        }

        public override void PrepareFrame(FrameData info)
        {
            if (StateChanged())
            {
                AssignAnimationsForStateChange();
                return;
            }
            if (InTransition())
            {
                SetTransitionWeightValues();
                return;
            }
            if (TransitionNeedsToStop())
            {
                RemoveTransitionAnimation();
            }
        }

        private void RemoveTransitionAnimation()
        {
            this.RemoveInput(0);
        }

        private bool TransitionNeedsToStop()
        {
            return this.inputCount > 1;
        }

        private void SetTransitionWeightValues()
        {
            if (_currentAnimation == null)
            {
                return;
            }
            if (this.inputCount < 2)
            {
                return;
            }
            var weight = Mathf.Clamp01((float)_currentAnimation.time / TransitionTime);
            this.SetInputWeight(0, 1 - weight);
            this.SetInputWeight(1, weight);
        }

        private void AssignAnimationsForStateChange()
        {
            ClearInputs();
            var previousState = _currentState;
            var state = State;
            if (state == null)
            {
                return;
            }
            var previousAnimation = _currentAnimation;
            if (!_animationPlayablesByState.TryGetValue(state.GetType(), out _currentAnimation))
            {
                return;
            }
            _currentAnimation.time = 0;
            if (previousAnimation != null)
            {
                AddInput(previousAnimation);
            }
            AddInput(_currentAnimation);

            if (previousAnimation != null)
            {
                this.SetInputWeight(0, TransitionStartFirstWeight);
                this.SetInputWeight(1, TransitionStartSecondWeight);
            }
        }

        public void Assign<TState>(AnimationPlayable animationPlayable) where TState : IState<TController, TAction>
        {
            _animationPlayablesByState.Add(typeof(TState), animationPlayable);
        }

        private bool StateChanged()
        {
            return _currentState != _stateManager.CurrentState;
        }

        private bool InTransition()
        {
            if (_currentAnimation != null && _currentAnimation.time < TransitionTime)
            {
                return true;
            }
            return false;
        }
    }
}
