using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace AnimatorSequencer.States.ControlStates
{
    [Serializable]
    public class UnityEventCallState : BaseSequenceState
    {
        [SerializeField]
        public UnityEvent call;

        protected override void PerformOnStateEnter()
        {
            call.Invoke();
        }

        public override bool Complete()
        {
            return true;
        }
    }
}
