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
    [Node(false, "Control/Change Camera Owner State")]
    public class ChangeCameraOwnerNode : BaseActionSequenceNode<ChangeCameraOwnerState>
    {
        public const string ID = "changeCameraOwnerNode";
        public override string GetID { get { return ID; } }

        public float value = 1f;

        public ChangeCameraOwnerNode() : base(CreateInstance<ChangeCameraOwnerState>())
        {
        }

        public override Node Create(Vector2 pos)
        {
            var node = CreateInstance<ChangeCameraOwnerNode>();

            node.name = "Change Camera Owner Node";
            node.BaseActionSequence.name = node.name;
            node.rect = new Rect(pos.x, pos.y, 200, 80); ;

            NodeInput.Create(node, "", "Float");
            NodeOutput.Create(node, "", "Float");

            return node;
        }
    }
}
