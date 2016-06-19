using GenericComponents.Animation.Playables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.Director;

namespace GenericComponents.Behaviours
{
    [RequireComponent(typeof(Animator))]
    public class PlayAnimation : MonoBehaviour
    {
        public AnimationClip animation;

        void Start()
        {
            var animator = GetComponent<Animator>();
            var crossfade = new CrossFadePlayable();
            animator.Play(crossfade);
            crossfade.Crossfade(new AnimationClipPlayable(animation), 2);
        }
    }
}
