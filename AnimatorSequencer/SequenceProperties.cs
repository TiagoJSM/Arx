using AnimatorSequencer.AnimationBehaviours;
using AnimatorSequencer.MovementBehaviours;
using AnimatorSequencer.Zones;
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
            var animator = GetComponent<Animator>();
            var animatorBehaviours = animator.GetBehaviours<StateMachineBehaviour>();

            foreach (var behaviourField in behaviourFields)
            {
                var animatorBehaviour = GetAnimatorBehaviour(animatorBehaviours, behaviourField);
                if(animatorBehaviour == null)
                {
                    continue;
                }
                var field =
                    animatorBehaviour
                        .GetType()
                        .GetField(behaviourField.FieldName);

                field.SetValue(animatorBehaviour, behaviourField.Value);
            }
            
            animator.enabled = false;
        }

        private StateMachineBehaviour GetAnimatorBehaviour(StateMachineBehaviour[] animatorBehaviours, BehaviourField behaviourField)
        {
            var stateBehaviours = 
                animatorBehaviours
                    .Where(ab => ab.GetType() == behaviourField.StateMachineBehaviour.GetType())
                    .ToArray();
            return stateBehaviours[behaviourField.Order];
        }
    }
}
