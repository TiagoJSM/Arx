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
    [Node(false, "Control/Enable All Player Control")]
    public class EnableAllPlayerControlNode : BaseActionSequenceNode<EnableAllPlayerControlState>
    {
        public const string ID = "enableAllPlayerControlNode";
        public override string GetID { get { return ID; } }

        public float value = 1f;

        public EnableAllPlayerControlNode() : base(CreateInstance<EnableAllPlayerControlState>())
        {
        }

        public override Node Create(Vector2 pos)
        { // This function has to be registered in Node_Editor.ContextCallback
            var node = CreateInstance<EnableAllPlayerControlNode>();

            node.name = "Enable All Player Control Node";
            node.BaseActionSequence.name = node.name;
            node.rect = new Rect(pos.x, pos.y, 200, 80); ;

            NodeInput.Create(node, "", "Float");
            NodeOutput.Create(node, "", "Float");

            return node;
        }
    }
}
