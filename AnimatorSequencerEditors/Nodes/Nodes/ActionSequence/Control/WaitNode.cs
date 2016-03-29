using AnimatorSequencer.States.ControlStates;
using CommonEditors.Nodes.Framework;
using CommonEditors.Nodes.Framework.CanvasSaveObjects;
using CommonEditors.Nodes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AnimatorSequencerEditors.Nodes.Nodes.ActionSequence.Control
{
    [Serializable]
    [Node(false, "Control/Wait")]
    public class WaitNode : BaseActionSequenceNode<WaitState>
    {
        public const string ID = "waitNode";
        public override string GetID { get { return ID; } }

        public float value = 1f;

        public WaitNode() : base(CreateInstance<WaitState>())
        {
        }

        public override Node Create(Vector2 pos)
        { 
            var node = CreateInstance<WaitNode>();

            node.name = "Wait Node";
            node.BaseActionSequence.name = node.name;
            node.rect = new Rect(pos.x, pos.y, 200, 80); ;

            NodeInput.Create(node, "", "Float");
            NodeOutput.Create(node, "", "Float");

            return node;
        }
    }
}
