using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer.States.ControlStates
{
    [Serializable]
    public class WaitState : BaseSequenceState
    {
        private float _startTime;

        [SerializeField]
        public float waitTimeInSeconds = 1;

        protected override void PerformOnStateEnter()
        {
            _startTime = Time.time;
        }

        public override bool Complete()
        {
            var elapsedtime = Time.time - _startTime;
            return elapsedtime >= waitTimeInSeconds;
        }
    }
}
