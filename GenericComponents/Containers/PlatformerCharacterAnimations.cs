using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Containers
{
    [Serializable]
    public class PlatformerCharacterAnimations
    {
        public AnimationClip iddleAnimation;
        public AnimationClip walkingAnimation;
        public AnimationClip runningAnimation;
        public AnimationClip duckingAnimation;
        public AnimationClip duckAnimation;
        public AnimationClip rollingAnimation;
        public AnimationClip jumpingAnimation;
        public AnimationClip grabbingAnimation;
    }
}
