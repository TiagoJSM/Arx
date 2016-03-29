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
    [Node(false, "Control/Change Active State")]
    public class ChangeActiveNode : BaseActionSequenceNode<ChangeActiveState>
    {
        public const string ID = "changeActiveState";
        public override string GetID { get { return ID; } }

        public float value = 1f;

        public ChangeActiveNode() : base(CreateInstance<ChangeActiveState>())
        {
        }

        public override Node Create(Vector2 pos)
        {
            var node = CreateInstance<DisableAllPlayerControlNode>();

            node.name = "Change Active State Node";
            node.BaseActionSequence.name = node.name;
            node.rect = new Rect(pos.x, pos.y, 200, 80); ;

            NodeInput.Create(node, "", "Float");
            NodeOutput.Create(node, "", "Float");

            return node;
        }
    }
}
