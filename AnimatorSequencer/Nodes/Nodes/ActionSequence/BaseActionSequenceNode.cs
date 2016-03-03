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

        public override ScriptableObject[] GetScriptableObjects()
        {
            return base.GetScriptableObjects().Concat(new[] { _actionSequence }).ToArray();
        }

        public override void CopyScriptableObjects(System.Func<ScriptableObject, ScriptableObject> replaceSerializableObject)
        {
            base.CopyScriptableObjects(replaceSerializableObject);
            _actionSequence = replaceSerializableObject(_actionSequence) as BaseSequenceState;
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
