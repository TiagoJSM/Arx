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
    public abstract class StateAnimationMixerPlayable<TState> : AnimationMixerPlayable
    {
        private const float TransitionTime = 0.3f;
        private const float TransitionStartFirstWeight = 0.99f;
        private const float TransitionStartSecondWeight = 0.01f;

        private TState _currentState;
        //private StateManager<TController, TAction> _stateManager;
        private Dictionary<TState, AnimationClipPlayable> _animationPlayablesByState;

        private AnimationClipPlayable _currentAnimation;

        public float NormalizedTime
        {
            get
            {
                return (float)_currentAnimation.time / _currentAnimation.clip.length;
            }
        }

        public float TimeToFinish
        {
            get
            {
                return _currentAnimation.clip.length - (float)_currentAnimation.time;
            }
        }

        public bool CurrentAnimationIsOver
        {
            get
            {
                return _currentAnimation.time > _currentAnimation.clip.length;
            }
        }

        private TState State
        {
            get
            {
                var state = GetState();
                if (_currentState == null)
                {
                    _currentState = state;
                }
                if (!_currentState.Equals(state))
                {
                    _currentState = state;
                }
                return _currentState;
            }
        }

        public StateAnimationMixerPlayable()
        {
            _animationPlayablesByState = new Dictionary<TState, AnimationClipPlayable>();
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
            if (!_animationPlayablesByState.TryGetValue(state, out _currentAnimation))
            {
                _animationPlayablesByState.TryGetValue(state, out _currentAnimation);
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

        public void Assign(TState state, AnimationClipPlayable animationPlayable)
        {
            _animationPlayablesByState.Add(state, animationPlayable);
        }

        private bool StateChanged()
        {
            var state = GetState();
            if(state == null)
            {
                return false;
            }
            return !GetState().Equals(_currentState);
        }

        private bool InTransition()
        {
            if (_currentAnimation != null && _currentAnimation.time < TransitionTime)
            {
                return true;
            }
            return false;
        }

        protected abstract TState GetState();
    }
}
