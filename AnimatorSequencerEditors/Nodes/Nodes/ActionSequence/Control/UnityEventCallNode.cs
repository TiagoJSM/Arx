using AnimatorSequencer.States.ControlStates;
using CommonEditors.Nodes.Framework;
using CommonEditors.Nodes.Framework.CanvasSaveObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencerEditors.Nodes.Nodes.ActionSequence.Control
{
    [Serializable]
    [Node(false, "Control/Unity Event Call")]
    public class UnityEventCallNode : BaseActionSequenceNode<UnityEventCallState>
    {
        public const string ID = "unityEventCallNode";
        public override string GetID { get { return ID; } }

        public float value = 1f;

        public UnityEventCallNode() : base(CreateInstance<UnityEventCallState>())
        {
        }

        public override Node Create(Vector2 pos)
        {
            var node = CreateInstance<UnityEventCallNode>();

            node.name = "Unity Event Call Node";
            node.BaseActionSequence.name = node.name;
            node.rect = new Rect(pos.x, pos.y, 200, 80); ;

            NodeInput.Create(node, "", "Float");
            NodeOutput.Create(node, "", "Float");

            return node;
        }
    }
}
