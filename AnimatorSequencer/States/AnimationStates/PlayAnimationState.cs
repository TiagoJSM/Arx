using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer.States.AnimationStates
{
    [Serializable]
    public class PlayAnimationState : BaseSequenceState
    {
        public Animator animator;

        protected override void PerformOnStateEnter()
        {
            animator.enabled = true;
            animator.Play(0);
        }

        public override bool Complete()
        {
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.normalizedTime > 1;
        }
    }
}
