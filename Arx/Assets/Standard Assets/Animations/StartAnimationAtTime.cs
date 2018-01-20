using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Animations
{
    [RequireComponent(typeof(Animator))]
    public class StartAnimationAtTime : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 1)]
        private float _normalizedStartTime = 0;

        void Awake()
        {
            var animator = GetComponent<Animator>();
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            animator.Play(stateInfo.fullPathHash, -1, _normalizedStartTime);
        }
    }
}
