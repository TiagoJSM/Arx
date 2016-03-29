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
    public class DisablePlayerControlState : BaseSequenceState
    {
        [SerializeField]
        public GameObject[] gameObjects;

        protected override void PerformOnStateEnter()
        {
            ObjectUtils.DisableAllBehaviours<IPlayerControl>(gameObjects);
        }

        public override bool Complete()
        {
            return true;
        }
    }
}
