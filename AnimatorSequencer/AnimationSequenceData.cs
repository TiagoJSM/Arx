using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer
{
    public class AnimationSequenceData : ScriptableObject
    {
        public List<AnimationSequenceNode> sequenceNodes;
        public AnimationSequenceNode root;
    }
}
