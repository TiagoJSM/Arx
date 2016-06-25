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

    public class PlatformerCharacterState
    {
        public Type StateType { get; set; }
        public int? ComboNumber { get; set; }

        public PlatformerCharacterState() { }

        public override bool Equals(object obj)
        {
            var other = obj as PlatformerCharacterState;
            if(other == null)
            {
                return false;
            }
            if (StateType != other.StateType)
            {
                return false;
            }
            if(ComboNumber == null || other.ComboNumber == null)
            {
                return true;
            }
            return ComboNumber == other.ComboNumber;
        }

        public override int GetHashCode()
        {
            return StateType != null ? StateType.GetHashCode() : 0;
        }
    }

    public class PlatformerCharacterStateAnimationPlayable : StateAnimationMixerPlayable<PlatformerCharacterState>
    {
        private PlatformerCharacterAnimations _animations;
        private StateManager<IPlatformerCharacterController, PlatformerCharacterAction> _stateManager;

        public PlatformerCharacterStateAnimationPlayable(
            StateManager<IPlatformerCharacterController, PlatformerCharacterAction> stateManager,
            PlatformerCharacterAnimations animations,
            float rollingDuration)
            : base()
        {
            _animations = animations;
            _stateManager = stateManager;

            var rollingPlayable = new AnimationClipPlayable(_animations.rollingAnimation);
            var time = rollingDuration / rollingPlayable.clip.length;
            rollingPlayable.speed = 1 / time;
            
            Assign(
                new PlatformerCharacterState() { StateType = typeof(IddleState) }, 
                new AnimationClipPlayable(_animations.iddleAnimation));
            Assign(
                new PlatformerCharacterState() { StateType = typeof(MovingState) }, 
                new AnimationClipPlayable(_animations.runningAnimation));
            Assign(
                new PlatformerCharacterState() { StateType = typeof(JumpingState) }, 
                new AnimationClipPlayable(_animations.jumpingAnimation));
            Assign(
                new PlatformerCharacterState() { StateType = typeof(FallingState) }, 
                new AnimationClipPlayable(_animations.fallingAnimation));
            Assign(
                new PlatformerCharacterState() { StateType = typeof(GrabbingLedgeState) }, 
                new AnimationClipPlayable(_animations.grabbingAnimation));
            Assign(
                new PlatformerCharacterState() { StateType = typeof(DuckState) }, 
                new AnimationClipPlayable(_animations.duckAnimation));
            Assign(
                new PlatformerCharacterState() { StateType = typeof(RollState) }, 
                rollingPlayable);
            Assign(
                new PlatformerCharacterState() { StateType = typeof(LightAttackGroundState), ComboNumber = 1 },
                new AnimationClipPlayable(_animations.lightAttack1));
            Assign(
                new PlatformerCharacterState() { StateType = typeof(LightAttackGroundState), ComboNumber = 2 },
                new AnimationClipPlayable(_animations.lightAttack2));
            Assign(
                new PlatformerCharacterState() { StateType = typeof(LightAttackGroundState), ComboNumber = 3 },
                new AnimationClipPlayable(_animations.lightAttack3));
        }

        protected override PlatformerCharacterState GetState()
        {
            var currentState = _stateManager.CurrentState;
            return new PlatformerCharacterState()
            {
                StateType = currentState == null ? default(Type) : currentState.GetType(),
                ComboNumber = _stateManager.Controller.ComboNumber
            };
        }
    }
}
