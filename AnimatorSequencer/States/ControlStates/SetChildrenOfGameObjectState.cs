using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer.States.ControlStates
{
    [Serializable]
    public class SetChildrenOfGameObjectState : BaseSequenceState
    {
        [SerializeField]
        public GameObject children;
        [SerializeField]
        public GameObject parent;

        protected override void PerformOnStateEnter()
        {
            children.transform.parent = parent == null? null : parent.transform;
        }

        public override bool Complete()
        {
            return true;
        }
    }
}
