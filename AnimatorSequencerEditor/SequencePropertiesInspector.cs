using AnimatorSequencer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace AnimatorSequencerEditor
{
    [CustomEditor(typeof(SequenceProperties))]
    public class SequencePropertiesInspector : Editor
    {
        private BindingFlags _fieldFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetField | BindingFlags.GetField;
        private SequenceProperties Target
        {
            get
            {
                return target as SequenceProperties;
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.indentLevel = 0;
            Undo.RecordObject(target, "Sequence properties changed");
            var animator = Target.GetComponent<Animator>();
            var controller = animator.runtimeAnimatorController as AnimatorController;
            if(controller == null)
            {
                Debug.LogError("Missing controller");
                return;
            }
            var baseLayer = controller.layers[0];
            RemoveNonExistentFields(baseLayer);
            foreach(var animatorState in baseLayer.stateMachine.states)
            {
                GenerateStateBehavioursEditionFields(animatorState.state, animatorState.state.behaviours);
            }            
        }

        private void RemoveNonExistentFields(AnimatorControllerLayer baseLayer)
        {
            var editorBehaviours =
                    baseLayer
                        .stateMachine
                        .states
                        .SelectMany(animStates => animStates.state.behaviours);

            var targetBehaviours = Target.behaviourFields.ToArray();
            foreach (var targetBehaviourField in targetBehaviours)
            {
                var editorBehaviour =
                    editorBehaviours
                        .Where(stateBehaviour => stateBehaviour == targetBehaviourField.StateMachineBehaviour)
                        .ToArray();

                if (!editorBehaviour.Any())
                {
                    Target.behaviourFields.Remove(targetBehaviourField);
                }

                var fieldExists = editorBehaviour.Any(eb => eb.GetType().GetField(targetBehaviourField.FieldName) != null);
                if (!fieldExists)
                {
                    Target.behaviourFields.Remove(targetBehaviourField);
                }
            }
        }

        private void GenerateStateBehavioursEditionFields(AnimatorState state, StateMachineBehaviour[] behaviours)
        {
            if(behaviours.Length == 0)
            {
                return;
            }

            var orderMapping = behaviours.Select(b => b.GetType()).Distinct().ToDictionary(t => t, t => 0);

            foreach (var behaviour in behaviours)
            {
                EditorGUILayout.InspectorTitlebar(true, behaviour);
                var fields = 
                    behaviour
                        .GetType()
                        .GetFields(_fieldFlags)
                        .Where(f => f.FieldType.IsSubclassOf(typeof(UnityEngine.Object)))
                        .ToArray();
                GenerateEditionFields(behaviour, fields, orderMapping);
                orderMapping[behaviour.GetType()] = orderMapping[behaviour.GetType()] + 1;
            }
        }

        private void GenerateEditionFields(StateMachineBehaviour behaviour, FieldInfo[] fields, Dictionary<Type, int> orderMapping)
        {
            foreach(var field in fields)
            {
                var behaviourField = Target.GetBehaviourField(behaviour, field.Name);
                if (behaviourField == null)
                {
                    behaviourField = Target.SetBehaviourField(behaviour, field.Name, null);
                }
                var obj = behaviourField.Value;
                if (obj == null)
                {
                    obj = field.GetValue(behaviour) as UnityEngine.Object;
                }
                EditorGUI.indentLevel = 1;
                var type = field.FieldType;
                behaviourField.Order = orderMapping[behaviour.GetType()];
                
                behaviourField.Value = EditorGUILayout.ObjectField(field.Name, obj, type, true);
            }
        }
    }
}
