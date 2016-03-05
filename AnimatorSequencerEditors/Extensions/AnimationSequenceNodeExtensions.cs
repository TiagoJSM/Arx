using AnimatorSequencer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimatorSequencerEditors.Extensions
{
    public static class AnimationSequenceNodeExtensions
    {
        public static IEnumerable<AnimationSequenceNode> GetAllSequenceNodes(this AnimationSequenceNode root)
        {
            var result = new List<AnimationSequenceNode>() { root };
            foreach (var nextState in root.nextStates)
            {
                result.AddRange(GetAllSequenceNodes(nextState));
            }
            return result;
        }
    }
}
