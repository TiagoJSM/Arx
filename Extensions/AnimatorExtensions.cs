using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Extensions
{
    public static class AnimatorExtensions
    {
        public static bool IsCurrentAnimationOver(this Animator animator)
        {
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;
        }
    }
}
