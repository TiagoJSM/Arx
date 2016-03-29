using CommonInterfaces.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using Utils;

namespace AnimatorSequencer.States.ControlStates
{
    [Serializable]
    public class DisableAllPlayerControlState : BaseSequenceState
    {
        protected override void PerformOnStateEnter()
        {
            ObjectUtils.DisableAllBehaviours<IPlayerControl>();
        }

        public override bool Complete()
        {
            return true;
        }
    }
}
