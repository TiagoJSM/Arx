using _2DDynamicCamera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer.States.ControlStates
{
    [Serializable]
    public class ChangeCameraOwnerState : BaseSequenceState
    {
        public Transform newOwner;
        public DynamicCamera camera;

        protected override void PerformOnStateEnter()
        {
            camera.Owner = newOwner;
        }

        public override bool Complete()
        {
            return true;
        }
    }
}
