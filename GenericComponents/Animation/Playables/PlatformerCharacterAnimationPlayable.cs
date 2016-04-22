using GenericComponents.Containers;
using GenericComponents.Interfaces.States.PlatformerCharacter;
using GenericComponents.StateMachine;
using GenericComponents.StateMachine.States.PlatformerCharacter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.Director;

namespace GenericComponents.Animation.Playables
{
    public class PlatformerCharacterAnimationPlayable : AnimationMixerPlayable
    {
        private AnimationPlayable _defaultAnimation;
        private AnimationPlayable _requestedAnimation;

        public PlatformerCharacterAnimationPlayable(AnimationPlayable defaultAnimation)
        {
            _defaultAnimation = defaultAnimation;
            PlayDefaultAnimation();
        }

        public override void PrepareFrame(FrameData info)
        {
            //PlayDefaultAnimation();
        }

        public void PlayAnimationOverDefault(AnimationClip animation)
        {
            _requestedAnimation = new AnimationClipPlayable(animation);
            ClearInputs();
            _requestedAnimation.time = 0;
            AddInput(_requestedAnimation);
            this.SetInputWeight(0, 1);
        }

        public void PlayDefaultAnimation()
        {
            _requestedAnimation = null;
            ClearInputs();
            _defaultAnimation.time = 0;
            AddInput(_defaultAnimation);
            this.SetInputWeight(0, 1);
        }
    }

    public class PlatformerCharacterStateAnimationPlayable : 
        StateAnimationMixerPlayable<IPlatformerCharacterController, PlatformerCharacterAction>
    {
        private PlatformerCharacterAnimations _animations;

        public PlatformerCharacterStateAnimationPlayable(
            StateManager<IPlatformerCharacterController, PlatformerCharacterAction> stateManager,
            PlatformerCharacterAnimations animations,
            float rollingDuration)
            : base(stateManager)
        {
            _animations = animations;

            var rollingPlayable = new AnimationClipPlayable(_animations.rollingAnimation);
            var time = rollingDuration / rollingPlayable.clip.length;
            rollingPlayable.speed = 1 / time;
            
            Assign<IddleState>(new AnimationClipPlayable(_animations.iddleAnimation));
            Assign<MovingState>(new AnimationClipPlayable(_animations.runningAnimation));
            Assign<JumpingState>(new AnimationClipPlayable(_animations.jumpingAnimation));
            Assign<FallingState>(new AnimationClipPlayable(_animations.fallingAnimation));
            Assign<GrabbingLedgeState>(new AnimationClipPlayable(_animations.grabbingAnimation));
            Assign<DuckState>(new AnimationClipPlayable(_animations.duckAnimation));
            Assign<RollState>(rollingPlayable);
        }
    }
}
