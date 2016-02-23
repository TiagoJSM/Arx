using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.Director;

namespace AnimatorSequencer.AnimationBehaviours
{
    public class PlayAnimationBehaviour : BaseStateMachineBehaviour
    {
        public Animator target;

        protected override void PerformOnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        protected override void PerformOnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        protected override void PerformStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        protected override void OnDelayPassed()
        {
            target.enabled = true;
            target.Play(0);
        }
    }
}
