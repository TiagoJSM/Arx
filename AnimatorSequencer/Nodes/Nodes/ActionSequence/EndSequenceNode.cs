using CommonEditors.Nodes.Framework;
using CommonEditors.Nodes.Framework.CanvasSaveObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer.Nodes.Nodes.ActionSequence
{
    [System.Serializable]
    [Node(false, "End sequence")]
    public class EndSequenceNode : Node
    {
        public const string ID = "endSequenceNode";
        public override string GetID { get { return ID; } }

        [HideInInspector]
        public bool assigned = false;
        public float value = 0;

        public override Node Create(Vector2 pos)
        { // This function has to be registered in Node_Editor.ContextCallback
            var node = CreateInstance<EndSequenceNode>();

            var areaStyle = new GUIStyle();
            var texture = new Texture2D(1, 1);
            texture.SetPixels(new Color[] { new Color(0.5f, 0.0f, 0f) });
            texture.Apply();
            node.BackgroundColorTexture = texture;
            node.canBeDeleted = false;

            node.name = "End Sequence Node";
            node.rect = new Rect(pos.x, pos.y, 150, 50);

            NodeInput.Create(node, "Value", "Float");

            return node;
        }

        protected override void NodeGUI()
        {
            Inputs[0].DisplayLayout(new GUIContent("Value : " + (assigned ? value.ToString() : ""), "The input value to display"));
        }

        public override bool Calculate()
        {
            if (!allInputsReady())
            {
                value = 0;
                assigned = false;
                return false;
            }

            value = Inputs[0].connection.GetValue<float>();
            assigned = true;

            return true;
        }
    }
}
