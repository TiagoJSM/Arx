using AnimatorSequencer.States.InteractionStates;
using CommonEditors.Nodes.Framework;
using CommonEditors.Nodes.Framework.CanvasSaveObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencerEditors.Nodes.Nodes.ActionSequence.Interaction
{
    [Serializable]
    [Node(false, "Interaction/Dialogue")]
    public class DialogueNode : BaseActionSequenceNode<DialogueState>
    {
        public const string ID = "dialogueNode";
        public override string GetID { get { return ID; } }

        public float value = 1f;

        public DialogueNode() : base(CreateInstance<DialogueState>())
        {
        }

        public override Node Create(Vector2 pos)
        {
            var node = CreateInstance<DialogueNode>();

            node.name = "Dialogue Node";
            node.BaseActionSequence.name = node.name;
            node.rect = new Rect(pos.x, pos.y, 200, 80); ;

            NodeInput.Create(node, "", "Float");
            NodeOutput.Create(node, "", "Float");

            return node;
        }
    }
}
