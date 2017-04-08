using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.GenericComponents.Animation
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorDelay : MonoBehaviour
    {
        [SerializeField]
        private float _delay = 10;

        void Awake()
        {
            var animator = GetComponent<Animator>();
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            animator.Play(stateInfo.fullPathHash, -1, _delay);
        }
    }
}
