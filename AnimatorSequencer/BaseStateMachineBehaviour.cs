using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer
{
    public abstract class BaseStateMachineBehaviour : StateMachineBehaviour
    {
        private float _stateEnterTime;
        private float _onDelayPassed;
        private bool _onDelayPassedCalled;

        public float delay;

        public float StateEnterTime
        {
            get
            {
                return _stateEnterTime;
            }
        }
        public float OnDelayPassedTime
        {
            get
            {
                return _onDelayPassed;
            }
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            _stateEnterTime = Time.time;
            _onDelayPassedCalled = false;
            PerformOnStateEnter(animator, stateInfo, layerIndex);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            var elapsed = Time.time - _stateEnterTime;
            if(elapsed < delay)
            {
                return;
            }
            if (!_onDelayPassedCalled)
            {
                _onDelayPassedCalled = true;
                _onDelayPassed = Time.time;
                OnDelayPassed();
            }
            PerformStateUpdate(animator, stateInfo, layerIndex);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            PerformOnStateExit(animator, stateInfo, layerIndex);
        }

        protected abstract void PerformOnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
        protected abstract void PerformStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
        protected abstract void PerformOnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
        protected virtual void OnDelayPassed() { }
    }
}
