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

        private float? _previousFixedUpdateTime;
        private float _currentFixedUpdateTime;

        private float? _previousUpdateTime;
        private float _currentUpdateTime;

        //[HideInInspector]
        [SerializeField]
        private string _id;

        public string Id
        {
            get
            {
                return _id;
            }
        }

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

        public float ElapsedTimeSinceLastFixedUpdate
        {
            get
            {
                return Time.time - _previousFixedUpdateTime.Value;
            }
        }

        public float ElapsedTimeSinceLastUpdate
        {
            get
            {
                return Time.time - _previousUpdateTime.Value;
            }
        }

        public BaseSequenceState()
        {
            _id = Guid.NewGuid().ToString();
        }

        public void OnStateEnter()
        {
            _stateEnterTime = Time.time;
            PerformOnStateEnter();
        }

        public void OnStateFixedUpdate()
        {
            _currentFixedUpdateTime = Time.time;
            if (_previousFixedUpdateTime == null)
            {
                _previousFixedUpdateTime = _currentFixedUpdateTime;
            }
            PerformOnStateFixedUpdate();
            _previousFixedUpdateTime = _currentFixedUpdateTime;
        }

        public void OnStateUpdate()
        {
            _currentUpdateTime = Time.time;
            if(_previousUpdateTime == null)
            {
                _previousUpdateTime = _currentUpdateTime;
            }
            PerformOnStateUpdate();
            _previousUpdateTime = _currentUpdateTime;
        }

        public void OnStateExit()
        {
            PerformOnStateExit();
        }

        public bool AreCopies(BaseSequenceState other)
        {
            return
                other.GetType() == this.GetType() &&
                this.Id != Guid.Empty.ToString() &&
                this.Id == other.Id;
        }

        public abstract bool Complete();

        protected virtual void PerformOnStateEnter() { }
        protected virtual void PerformOnStateFixedUpdate() { }
        protected virtual void PerformOnStateUpdate() { }
        protected virtual void PerformOnStateExit() { }
    }
}
