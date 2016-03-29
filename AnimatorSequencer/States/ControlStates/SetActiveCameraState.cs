using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utils;

namespace AnimatorSequencer.States.ControlStates
{
    [Serializable]
    public class SetActiveCameraState : BaseSequenceState
    {
        public Camera camera;

        protected override void PerformOnStateEnter()
        {
            CameraUtils.SetActiveCamera(camera);
        }

        public override bool Complete()
        {
            return true;
        }
    }
}
