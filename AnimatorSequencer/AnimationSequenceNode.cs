using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer
{
    [Serializable]
    public class AnimationSequenceNode : ScriptableObject
    {
        [SerializeField]
        public BaseSequenceState state;
        [SerializeField]
        public List<AnimationSequenceNode> nextStates;

        public AnimationSequenceNode()
        {
            nextStates = new List<AnimationSequenceNode>();
        }
    }
}
