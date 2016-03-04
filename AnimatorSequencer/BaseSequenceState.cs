﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer
{
    public abstract class BaseSequenceState : ScriptableObject
    {
        private float _stateEnterTime;

        public float StateEnterTime
        {
            get
            {
                return _stateEnterTime;
            }
        }

        public void OnStateEnter()
        {
            _stateEnterTime = Time.time;
            PerformOnStateEnter();
        }

        public void OnStateUpdate()
        {
            PerformOnStateUpdate();
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
