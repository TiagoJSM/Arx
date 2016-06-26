using GenericComponents.Animation.Playables;
using GenericComponents.Containers;
using GenericComponents.Controllers.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Controllers.AnimationControllers
{
    [RequireComponent(typeof(PlatformerCharacterController))]
    [RequireComponent(typeof(Animator))]
    public class PlatformerCharacterAnimationController : MonoBehaviour
    {
        private Animator _animator;
        private PlatformerCharacterStateAnimationPlayable _statePlayable;
        private PlatformerCharacterAnimationPlayable _characterAnimations;

        [SerializeField]
        public PlatformerCharacterAnimations animations;
        public float rollingDuration = 1;

        public bool IsCurrentAnimationOver
        {
            get
            {
                return _statePlayable.NormalizedTime >= 1;
            }
        }

        public void PlayAnimation(AnimationClip animation)
        {
            _characterAnimations.PlayAnimationOverDefault(animation);
        }

        public void PlayCharacterAnimations()
        {
            _characterAnimations.PlayDefaultAnimation();
        }

        void Start()
        {
            _animator = GetComponent<Animator>();
            var stateManager = GetComponent<PlatformerCharacterController>().StateManager;
            _statePlayable = new PlatformerCharacterStateAnimationPlayable(stateManager, animations, rollingDuration);
            _characterAnimations = new PlatformerCharacterAnimationPlayable(_statePlayable);

            _animator.Play(_characterAnimations);
        }
    }
}
