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

            if(myTarget.root != null)
            {
                myTarget.clonedRoot = CloneRoot(myTarget.root);
                myTarget.root = null;
                SetFoldouts(myTarget);
            }

            if (myTarget.clonedRoot == null)
            {
                _foldouts = null;
                return;
            }

            if(_foldouts == null)
            {
                SetFoldouts(myTarget);
            }

            base.OnInspectorGUI();

            //ToDo: Delete removed from Behaviour
            DisplayStates(myTarget);

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }

        private void DisplayStates(AnimationSequenceBehaviour behaviour)
        {
            var nodes = GetStates(behaviour);

            foreach (var node in nodes)
            {
                Undo.RecordObject(node.Item2, "Property change");
                EditorGUI.indentLevel = 1;
                _foldouts[node.Item2] = EditorGUILayout.Foldout(_foldouts[node.Item2], node.Item1);
                if (_foldouts[node.Item2])
                {
                    var serializedObject = new SerializedObject(node.Item2);
                    var iter = serializedObject.GetIterator();
                    if(iter == null)
                    {
                        continue;
                    }
                    iter.NextVisible(true);         //required by the API
                    while(iter.NextVisible(false))  // skip the first property, it's script, we don't want it to display
                    {
                        EditorGUILayout.PropertyField(iter);
                        serializedObject.ApplyModifiedProperties();
                    }
                }
            }
        }

        private AnimationSequenceNode CloneRoot(AnimationSequenceNode root)
        {
            var clone = Instantiate(root);
            clone.name = root.name;
            if (clone.nextStates == null)
            {
                return clone;
            }
            if(clone.state != null)
            {
                clone.state = Instantiate(root.state);
                clone.state.name = root.state.name;
            }
            clone.nextStates = root.nextStates.Select(CloneRoot).ToList();
            return clone;
        }

        private Tuple<string, BaseSequenceState>[] GetStates(AnimationSequenceBehaviour target)
        {
            return
                target
                    .clonedRoot
                    .GetAllSequenceNodes()
                    .Where(n => n.state != null)
                    .Select(n => new Tuple<string, BaseSequenceState>(n.state.name, n.state))
                    .ToArray();
        }

        private void SetFoldouts(AnimationSequenceBehaviour target)
        {
            var nodes = GetStates(target);

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
