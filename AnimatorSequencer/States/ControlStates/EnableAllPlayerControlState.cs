using CommonInterfaces.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utils;

namespace AnimatorSequencer.States.ControlStates
{
    [Serializable]
    public class EnableAllPlayerControlState : BaseSequenceState
    {
        protected override void PerformOnStateEnter()
        {
            ObjectUtils.EnableAllBehaviours<IPlayerControl>();
        }

        public override bool Complete()
        {
            return true;
        }
    }
}
