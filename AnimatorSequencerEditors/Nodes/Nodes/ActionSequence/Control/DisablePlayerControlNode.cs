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
    [Node(false, "Control/Disable Player Control")]
    public class DisablePlayerControlNode : BaseActionSequenceNode<DisablePlayerControlState>
    {
        public const string ID = "disablePlayerControlNode";
        public override string GetID { get { return ID; } }

        public float value = 1f;

        public DisablePlayerControlNode() : base(CreateInstance<DisablePlayerControlState>())
        {
        }

        public override Node Create(Vector2 pos)
        { // This function has to be registered in Node_Editor.ContextCallback
            var node = CreateInstance<DisablePlayerControlNode>();

            node.name = "Disable Player Control Node";
            node.BaseActionSequence.name = node.name;
            node.rect = new Rect(pos.x, pos.y, 200, 80); ;

            NodeInput.Create(node, "", "Float");
            NodeOutput.Create(node, "", "Float");

            return node;
        }
    }
}
