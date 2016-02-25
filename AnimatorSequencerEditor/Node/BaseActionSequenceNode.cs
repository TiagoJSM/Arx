using AnimatorSequencer;
using CommonEditors.Nodes.Framework;
using CommonEditors.Nodes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencerEditor.Nodes
{
    public abstract class BaseActionSequenceNode<TSequenceState> : Node where TSequenceState : BaseSequenceState
    {
        [SerializeField]
        private TSequenceState _sequenceState;

        protected TSequenceState SequenceState
        {
            get
            {
                return _sequenceState;
            }
        }

        public BaseActionSequenceNode(TSequenceState sequenceState)
        {
            _sequenceState = sequenceState;
        }
    }
}
