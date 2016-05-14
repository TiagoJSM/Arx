using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Behaviours
{
    [RequireComponent(typeof(Animator))]
    public class AnimationRandomStartTime : MonoBehaviour
    {
        void Awake()
        {
            var animator = GetComponent<Animator>();
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            var random = UnityEngine.Random.Range(0f, 1.0f);
            animator.Play(stateInfo.fullPathHash, -1, random);
        }
    }
}
