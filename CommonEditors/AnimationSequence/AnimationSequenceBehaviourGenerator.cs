using AnimatorSequencer;
using AnimatorSequencer.Nodes.Nodes.ActionSequence;
using CommonEditors.Nodes.Framework;
using CommonEditors.Nodes.Framework.CanvasSaveObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CommonEditors.AnimationSequence
{
    public static class AnimationSequenceBehaviourGenerator
    {
        public static AnimationSequenceNode Compile(NodeCanvas canvas)
        {
            var startNode = GetStartNode(canvas);
            if(startNode == null)
            {
                Debug.LogError("No 'StartSequenceNode' found, to generate an animation sequence a 'StartSequenceNode' is needed as an animation entry point");
                return null;
            }
            var root = ScriptableObject.CreateInstance<AnimationSequenceNode>();
            PopulateNode(root, startNode.OutputConnectedNodes);
            return root;
        }

        private static void PopulateNode(AnimationSequenceNode target, IEnumerable<Node> nextNodes)
        {
            var nextActionSequenceNodes = nextNodes.OfType<BaseActionSequenceNode>().ToArray();
            foreach (var sequenceNode in nextActionSequenceNodes)
            {
                var actionState = ScriptableObject.CreateInstance(sequenceNode.BaseActionSequence.GetType()) as BaseSequenceState;
                var nextNode = ScriptableObject.CreateInstance<AnimationSequenceNode>();
                nextNode.state = actionState;
                PopulateNode(nextNode, sequenceNode.OutputConnectedNodes);
                target.nextStates.Add(nextNode);
            }
        }

        private static StartSequenceNode GetStartNode(NodeCanvas canvas)
        {
            return canvas.nodes.OfType<StartSequenceNode>().FirstOrDefault();
        }
    }
}
