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
    [Node(false, "Movement/Timed rotation")]
    public class TimedRotationNode : BaseActionSequenceNode<TimedRotationState>
    {
        public const string ID = "timedRotationNode";
        public override string GetID { get { return ID; } }

        //public float value = 1f;

        public TimedRotationNode() : base(CreateInstance<TimedRotationState>())
        {
        }

        public override Node Create(Vector2 pos)
        { // This function has to be registered in Node_Editor.ContextCallback
            var node = CreateInstance<TimedRotationNode>();

            node.name = "Timed Rotation Node";
            node.BaseActionSequence.name = node.name;
            node.rect = new Rect(pos.x, pos.y, 200, 80);

            NodeInput.Create(node, "", "Float");
            NodeOutput.Create(node, "", "Float");

            return node;
        }
    }
}
