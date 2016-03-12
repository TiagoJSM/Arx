using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer
{
    public class AnimTest : MonoBehaviour
    {
        public Animator anim;

        void Update()
        {
            var currentClip = anim.GetCurrentAnimatorClipInfo(0);
            var stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            var nextState = anim.GetNextAnimatorStateInfo(0);
            //currentClip = currentClip;
            Debug.Log(nextState);
        }
    }
}
