using AnimatorSequencer;
using CommonEditors.Nodes.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencerEditors.Nodes.Nodes.ActionSequence
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

        protected override void NodeGUI()
        {
            GUILayout.BeginHorizontal();
            Inputs[0].DisplayLayout();

            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("State name");
            this.BaseActionSequence.name = GUILayout.TextField(this.BaseActionSequence.name);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            Outputs[0].DisplayLayout();
            GUILayout.EndHorizontal();

            if (GUI.changed)
                NodeEditor.RecalculateFrom(this);
        }

        public override bool Calculate()
        {
            Outputs[0].SetValue<float>(0);
            return true;
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
