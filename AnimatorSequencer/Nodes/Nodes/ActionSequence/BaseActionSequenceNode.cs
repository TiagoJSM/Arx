using CommonEditors.Nodes.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer.Nodes.Nodes.ActionSequence
{
    public abstract class BaseActionSequenceNode : Node
    {
        [SerializeField]
        private BaseSequenceState _actionSequence;

        public BaseSequenceState BaseActionSequence
        {
            get
            {
                return _actionSequence;
            }
        }

        public BaseActionSequenceNode(BaseSequenceState actionSequence)
        {
            _actionSequence = actionSequence;
        }
    }

    public abstract class BaseActionSequenceNode<TActionSequence> : BaseActionSequenceNode where TActionSequence : BaseSequenceState
    {
        public TActionSequence ActionSequence
        {
            get
            {
                return BaseActionSequence as TActionSequence;
            }
        }

        public BaseActionSequenceNode(TActionSequence actionSequence)
            :base(actionSequence)
        {
        }
    }
}
