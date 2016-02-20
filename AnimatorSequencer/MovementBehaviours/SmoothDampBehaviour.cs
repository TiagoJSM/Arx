using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer.MovementBehaviours
{
    public class SmoothDampBehaviour : BaseStateMachineBehaviour
    {
        public Transform moveToPosition;
        public Transform target;
        public float time = 1;
        public float maxSpeed = Mathf.Infinity;

        private Vector3 _velocity;

        protected override void PerformOnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.applyRootMotion = true;
        }

        protected override void PerformStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            target.position = Vector3.SmoothDamp(target.position, moveToPosition.position, ref _velocity, time, maxSpeed);
        }

        protected override void PerformOnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }
    }
}
