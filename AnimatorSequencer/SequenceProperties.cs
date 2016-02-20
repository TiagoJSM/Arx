using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.Animations;
using UnityEngine;

namespace AnimatorSequencer
{
    [RequireComponent(typeof(Animator))]
    public class SequenceProperties : MonoBehaviour
    {
        [SerializeField]
        public List<BehaviourField> behaviourFields;

        public SequenceProperties()
        {
            behaviourFields = new List<BehaviourField>();
        }

        public BehaviourField SetBehaviourField(StateMachineBehaviour behaviour, string fieldName, UnityEngine.Object value)
        {
            var storedBehaviour = 
                behaviourFields
                    .FirstOrDefault(bf => 
                    bf.StateMachineBehaviour == behaviour && 
                    bf.FieldName == fieldName);
            if(storedBehaviour != null)
            {
                storedBehaviour.Value = value;
                return null;
            }
            storedBehaviour = new BehaviourField()
            {
                StateMachineBehaviour = behaviour,
                FieldName = fieldName,
                Value = value
            };
            behaviourFields.Add(storedBehaviour);
            return storedBehaviour;
        }

        public BehaviourField GetBehaviourField(StateMachineBehaviour behaviour, string fieldName)
        {
            return
                behaviourFields
                    .FirstOrDefault(bf =>
                        bf.StateMachineBehaviour == behaviour &&
                        bf.FieldName == fieldName);
        }

        void Start()
        {
            foreach(var behaviourField in behaviourFields)
            {
                var field =
                    behaviourField
                        .StateMachineBehaviour
                        .GetType()
                        .GetField(behaviourField.FieldName);
                field.SetValue(behaviourField.StateMachineBehaviour, behaviourField.Value);
            }
        }
    }
}
