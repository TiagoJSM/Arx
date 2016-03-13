using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer
{
    public abstract class BaseSequenceState : ScriptableObject
    {
        private float _stateEnterTime;
        private float? _previousTime;
        private float _currentTime;

        public float StateEnterTime
        {
            get
            {
                return _stateEnterTime;
            }
        }

        public float ElapsedTime
        {
            get
            {
                return Time.time - _stateEnterTime;
            }
        }

        public float ElapsedTimeSinceLastUpdate
        {
            get
            {
                return Time.time - _previousTime.Value;
            }
        }

        public void OnStateEnter()
        {
            _stateEnterTime = Time.time;
            PerformOnStateEnter();
        }

        public void OnStateUpdate()
        {
            _currentTime = Time.time;
            if(_previousTime == null)
            {
                _previousTime = _currentTime;
            }
            PerformOnStateUpdate();
            _previousTime = _currentTime;
        }

        public void OnStateExit()
        {
            PerformOnStateExit();
        }

        public abstract bool Complete();

        protected virtual void PerformOnStateEnter() { }
        protected virtual void PerformOnStateUpdate() { }
        protected virtual void PerformOnStateExit() { }
    }
}
