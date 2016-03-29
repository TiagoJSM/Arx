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
    [Node(false, "Control/Disable All Player Control")]
    public class DisableAllPlayerControlNode : BaseActionSequenceNode<DisableAllPlayerControlState>
    {
        public const string ID = "disableAllPlayerControlNode";
        public override string GetID { get { return ID; } }

        public float value = 1f;

        public DisableAllPlayerControlNode() : base(CreateInstance<DisableAllPlayerControlState>())
        {
        }

        public override Node Create(Vector2 pos)
        { // This function has to be registered in Node_Editor.ContextCallback
            var node = CreateInstance<DisableAllPlayerControlNode>();

            node.name = "Disable All Player Control Node";
            node.BaseActionSequence.name = node.name;
            node.rect = new Rect(pos.x, pos.y, 200, 80); ;

            NodeInput.Create(node, "", "Float");
            NodeOutput.Create(node, "", "Float");

            return node;
        }
    }
}
