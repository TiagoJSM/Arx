﻿using GenericComponents.Interfaces;
using GenericComponents.Interfaces.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.StateMachine
{
    public class StateManager<TController, TAction> : IStateFactory<TController, TAction>
    {
        private TController _controller;

        private StateContainer<TController, TAction> _root;
        private StateContainer<TController, TAction> _currentStateContainer;

        private Dictionary<Type, StateContainer<TController, TAction>> _states;

        public TController Controller
        {
            get
            {
                return _controller;
            }
        }

        public IState<TController, TAction> CurrentState
        {
            get
            {
                if(_currentStateContainer == null)
                {
                    return null;
                }
                return _currentStateContainer.State;
            }
        }

        public StateManager(TController controller)
        {
            _controller = controller;
            _states = new Dictionary<Type, StateContainer<TController, TAction>>();
        }

        public void Perform(TAction action)
        {
            if (_currentStateContainer == null)
            {
                if(_root == null)
                {
                    return;
                }
                _currentStateContainer = _root;
                _currentStateContainer.State.OnStateEnter(action);
            }

            var current = _currentStateContainer;
            current.State.TimeInState += Time.deltaTime;

            var child = current.Transitions.FirstOrDefault(t => t.Condition(_controller, action, current.State.TimeInState));
            if(child != null)
            {
                var transictionEvent = current.EventWhenTransitionTo.FirstOrDefault(c => c.Type == child.State.GetType());
                current = child.StateContainer;
                _currentStateContainer.State.OnStateExit(action);

                if(transictionEvent != null)
                {
                    transictionEvent.Action(Controller, action);           
                }

                _currentStateContainer = current;
                _currentStateContainer.State.TimeInState = 0;
                _currentStateContainer.State.OnStateEnter(action);
            }

            //Debug.Log(_currentStateContainer.State.GetType());
            _currentStateContainer.State.Perform(action);
            
        }

        public TransitionConfiguration<TController, TAction> SetInitialState<TState>() where TState : class, IState<TController, TAction>, new()
        {
            var state = State<TState>();
            var container = _states[state.GetType()];
            _root = container;
            //_currentStateContainer = _root;
            return new TransitionConfiguration<TController, TAction>(this, _root);
        }

        public TransitionConfiguration<TController, TAction> From<TState>() where TState : class, IState<TController, TAction>, new()
        {
            var state = State<TState>();
            var container = _states[state.GetType()];
            return new TransitionConfiguration<TController, TAction>(this, container);
        }

        public TState State<TState>() where TState : class, IState<TController, TAction>, new()
        {
            var type = typeof(TState);
            StateContainer<TController, TAction> cachedState;
            if (_states.TryGetValue(type, out cachedState))
            {
                return cachedState.State as TState;
            }
            var state = new TState()
            {
                StateController = _controller
            };

            var stateContainer = new StateContainer<TController, TAction>(state);
            _states.Add(type, stateContainer);
            return state;
        }

        public StateContainer<TController, TAction> GetContainer<TState>() where TState : class, IState<TController, TAction>, new()
        {
            var state = State<TState>();
            return _states[state.GetType()];
        }
    }
}
