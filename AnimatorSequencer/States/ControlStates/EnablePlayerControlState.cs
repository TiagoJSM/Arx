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
    public class EnablePlayerControlState : BaseSequenceState
    {
        [SerializeField]
        public GameObject[] gameObjects;

        protected override void PerformOnStateEnter()
        {
            ObjectUtils.EnableAllBehaviours<IPlayerControl>(gameObjects);
        }

        public override bool Complete()
        {
            return true;
        }
    }
}
