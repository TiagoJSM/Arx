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
    [Node(false, "Control/Set Active Camera Node")]
    public class SetActiveCameraNode : BaseActionSequenceNode<SetActiveCameraState>
    {
        public const string ID = "setActiveCameraNode";
        public override string GetID { get { return ID; } }

        public float value = 1f;

        public SetActiveCameraNode() : base(CreateInstance<SetActiveCameraState>())
        {
        }

        public override Node Create(Vector2 pos)
        {
            var node = CreateInstance<DisableAllPlayerControlNode>();

            node.name = "Set Active Camera Node";
            node.BaseActionSequence.name = node.name;
            node.rect = new Rect(pos.x, pos.y, 200, 80); ;

            NodeInput.Create(node, "", "Float");
            NodeOutput.Create(node, "", "Float");

            return node;
        }
    }
}
