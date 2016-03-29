using AnimatorSequencer.MovementStates;
using AnimatorSequencerEditors.Nodes.Nodes.ActionSequence;
using CommonEditors.Nodes.Framework;
using CommonEditors.Nodes.Framework.CanvasSaveObjects;
using CommonEditors.Nodes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AnimatorSequencerEditors.Nodes.Nodes.Movement
{
    [Serializable]
    [Node(false, "Movement/Lerp")]
    public class LerpNode : BaseActionSequenceNode<LerpState>
    {
        public const string ID = "lerpNode";
        public override string GetID { get { return ID; } }

        public LerpNode() : base(CreateInstance<LerpState>())
        {
        }

        public override Node Create(Vector2 pos)
        { // This function has to be registered in Node_Editor.ContextCallback
            var node = CreateInstance<LerpNode>();

            node.name = "Lerp Node";
            node.BaseActionSequence.name = node.name;
            node.rect = new Rect(pos.x, pos.y, 200, 80);

            NodeInput.Create(node, "", "Float");
            NodeOutput.Create(node, "", "Float");

            return node;
        }
    }
}
