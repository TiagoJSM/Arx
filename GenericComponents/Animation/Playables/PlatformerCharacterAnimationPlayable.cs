using GenericComponents.Containers;
using GenericComponents.Interfaces.States.PlatformerCharacter;
using GenericComponents.StateMachine;
using GenericComponents.StateMachine.States.PlatformerCharacter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Experimental.Director;

namespace GenericComponents.Animation.Playables
{
    public class PlatformerCharacterAnimationPlayable : 
        StateAnimationMixerPlayable<IPlatformerCharacterController, PlatformerCharacterAction>
    {
        private PlatformerCharacterAnimations _animations;

        public PlatformerCharacterAnimationPlayable(
            StateManager<IPlatformerCharacterController, PlatformerCharacterAction> stateManager,
            PlatformerCharacterAnimations animations)
            : base(stateManager)
        {
            _animations = animations;
            Assign<IddleState>(new AnimationClipPlayable(_animations.iddleAnimation));
            Assign<MovingState>(new AnimationClipPlayable(_animations.runningAnimation));
            Assign<JumpingState>(new AnimationClipPlayable(_animations.jumpingAnimation));
            Assign<FallingState>(new AnimationClipPlayable(_animations.jumpingAnimation));
            Assign<GrabbingLedgeState>(new AnimationClipPlayable(_animations.grabbingAnimation));
            Assign<DuckState>(new AnimationClipPlayable(_animations.duckAnimation));
            Assign<RollState>(new AnimationClipPlayable(_animations.rollingAnimation));
        }
    }
}
