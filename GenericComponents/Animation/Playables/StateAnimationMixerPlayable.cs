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
        private IState<TController, TAction> _currentState;
        private StateManager<TController, TAction> _stateManager;
        private Dictionary<Type, AnimationPlayable> _animationPlayablesByState;

        private IState<TController, TAction> State
        {
            get
            {
                if(_currentState == null)
                {
                    _currentState = _stateManager.CurrentState;
                }
                if(_currentState != _stateManager.CurrentState)
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
            if (!RequiresAnimationChange())
            {
                return;
            }
            ClearInputs();
            var state = State;
            if (state == null)
            {
                return;
            }
            AnimationPlayable playable;
            if (!_animationPlayablesByState.TryGetValue(state.GetType(), out playable))
            {
                return;
            }
            AddInput(playable);
        }

        public void Assign<TState>(AnimationPlayable animationPlayable) where TState : IState<TController, TAction>
        {
            _animationPlayablesByState.Add(typeof(TState), animationPlayable);
        }

        private bool RequiresAnimationChange()
        {
            return _currentState != _stateManager.CurrentState;
        }
    }
}
