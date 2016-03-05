using AnimatorSequencer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using Extensions;
using UnityEngine;
using System.Reflection;
using AnimatorSequencerEditors.Extensions;

namespace AnimatorSequencerEditors.AnimationSequence
{
    [CustomEditor(typeof(AnimationSequenceBehaviour))]
    public class AnimationSequenceEditor : Editor
    {
        private Dictionary<BaseSequenceState, bool> _foldouts;

        public override void OnInspectorGUI()
        {
            var myTarget = (AnimationSequenceBehaviour)target;
            var previous = myTarget.root;
            Undo.RecordObject(target, "Property change");

            myTarget.root = EditorGUILayout.ObjectField("Animation Sequence", myTarget.root, typeof(AnimationSequenceNode), true) as AnimationSequenceNode;

            if (myTarget.root == null)
            {
                _foldouts = null;
                return;
            }

            var nodes =
                myTarget
                    .root
                    .GetAllSequenceNodes()
                    .Where(n => n.state != null)
                    .Select(n => new Tuple<string, BaseSequenceState>(n.state.name, n.state))
                    .ToArray();

            if (previous != myTarget.root || _foldouts == null)
            {
                SetFoldouts(nodes);
            }

            
            foreach (var node in nodes)
            {
                EditorGUI.indentLevel = 1;
                _foldouts[node.Item2] = EditorGUILayout.Foldout(_foldouts[node.Item2], node.Item1);
                if (_foldouts[node.Item2])
                {
                    DisplayProperties(node.Item2, myTarget);
                }
            }
        }

        private void DisplayProperties(BaseSequenceState state, AnimationSequenceBehaviour behaviour)
        {
            var objectFields = 
                state
                    .GetType()
                    .GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .Where(f => typeof(UnityEngine.Object).IsAssignableFrom(f.FieldType))
                    .ToArray();

            foreach (var objectField in objectFields)
            {
                var stateParameter = behaviour.GetParameter(state, objectField.Name);
                stateParameter.value = EditorGUILayout.ObjectField(objectField.Name, stateParameter.value as UnityEngine.Object, objectField.FieldType, true);
                //behaviour.SetParameters(state, objectField.Name, value);
            }
        }

        private void SetFoldouts(Tuple<string, BaseSequenceState>[] nodes)
        {
            var foldouts = nodes.ToDictionary(n => n.Item2, n => true);
            if(_foldouts == null)
            {
                _foldouts = foldouts;
                return;
            }
            foreach(var kvp in _foldouts)
            {
                if (foldouts.ContainsKey(kvp.Key))
                {
                    foldouts[kvp.Key] = kvp.Value;
                }
            }
            _foldouts = foldouts;
        }
    }
}
