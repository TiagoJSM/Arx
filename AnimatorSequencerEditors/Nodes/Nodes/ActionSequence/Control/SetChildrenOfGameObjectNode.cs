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
    [Node(false, "Control/Set Children Of Game Object")]
    public class SetChildrenOfGameObjectNode : BaseActionSequenceNode<SetChildrenOfGameObjectState>
    {
        public const string ID = "setChildrenOfGameObject";
        public override string GetID { get { return ID; } }

        public float value = 1f;

        public SetChildrenOfGameObjectNode() : base(CreateInstance<SetChildrenOfGameObjectState>())
        {
        }

        public override Node Create(Vector2 pos)
        {
            var node = CreateInstance<SetChildrenOfGameObjectNode>();

            node.name = "Set Children Of Game Object Node";
            node.BaseActionSequence.name = node.name;
            node.rect = new Rect(pos.x, pos.y, 200, 80); ;

            NodeInput.Create(node, "", "Float");
            NodeOutput.Create(node, "", "Float");

            return node;
        }
    }
}
