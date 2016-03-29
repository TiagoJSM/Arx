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
    [Node(false, "Control/Enable Player Control")]
    public class EnablePlayerControlNode : BaseActionSequenceNode<EnablePlayerControlState>
    {
        public const string ID = "enablePlayerControlNode";
        public override string GetID { get { return ID; } }

        public float value = 1f;

        public EnablePlayerControlNode() : base(CreateInstance<EnablePlayerControlState>())
        {
        }

        public override Node Create(Vector2 pos)
        { // This function has to be registered in Node_Editor.ContextCallback
            var node = CreateInstance<EnablePlayerControlNode>();

            node.name = "Enable Player Control Node";
            node.BaseActionSequence.name = node.name;
            node.rect = new Rect(pos.x, pos.y, 200, 80); ;

            NodeInput.Create(node, "", "Float");
            NodeOutput.Create(node, "", "Float");

            return node;
        }
    }
}
