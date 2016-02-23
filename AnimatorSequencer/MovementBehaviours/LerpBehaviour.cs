using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer.MovementBehaviours
{
    public class LerpBehaviour : BaseStateMachineBehaviour
    {
        private Vector3 _startPosition;

        public Transform moveToPosition;
        public Transform target;
        public float time = 1;

        protected override void PerformOnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.applyRootMotion = true;
            _startPosition = target.position;
        }

        protected override void PerformStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var elapsed = Time.time - OnDelayPassedTime;
            var elapsedPercentage = elapsed / time;
            target.position = Vector3.Lerp(_startPosition, moveToPosition.position, elapsedPercentage);
        }

        protected override void PerformOnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }
    }
}
