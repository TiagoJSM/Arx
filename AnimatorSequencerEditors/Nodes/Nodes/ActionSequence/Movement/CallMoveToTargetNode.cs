using AnimatorSequencer.States.MovementStates;
using CommonEditors.Nodes.Framework;
using CommonEditors.Nodes.Framework.CanvasSaveObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencerEditors.Nodes.Nodes.ActionSequence.Movement
{
    [Serializable]
    [Node(false, "Movement/Call Move To Target")]
    public class CallMoveToTargetNode : BaseActionSequenceNode<CallMoveToTargetState>
    {
        public const string ID = "callMoveToTargetNode";
        public override string GetID { get { return ID; } }

        public CallMoveToTargetNode() : base(CreateInstance<CallMoveToTargetState>())
        {
        }

        public override Node Create(Vector2 pos)
        {
            var node = CreateInstance<CallMoveToTargetNode>();

            node.name = "Call Move To Target Node";
            node.BaseActionSequence.name = node.name;
            node.rect = new Rect(pos.x, pos.y, 200, 80);

            NodeInput.Create(node, "", "Float");
            NodeOutput.Create(node, "", "Float");

            return node;
        }
    }
}
