using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer.MovementBehaviours
{
    public class LerpBehaviour : BaseStateMachineBehaviour
    {
        public Transform moveToPosition;
        public Transform target;
        public float time = 1;

        protected override void PerformOnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.applyRootMotion = true;
        }

        protected override void PerformOnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var elapsed = Time.time - StateEnterTime;
            var elapsedPercentage = elapsed / time;
            target.position = Vector3.Lerp(target.position, moveToPosition.position, elapsedPercentage);
        }

        protected override void PerformStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }
    }
}
