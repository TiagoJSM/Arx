using CommonEditors.Nodes.Framework;
using CommonEditors.Nodes.Framework.CanvasSaveObjects;
using CommonEditors.Nodes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CommonEditors.Nodes.Nodes
{
    [System.Serializable]
    [Node(false, "Movement/Lerp")]
    public class LerpNode : Node
    {
        public const string ID = "lerpNode";
        public override string GetID { get { return ID; } }

        public float value = 1f;

        public override Node Create(Vector2 pos)
        { // This function has to be registered in Node_Editor.ContextCallback
            var node = CreateInstance<LerpNode>();

            node.name = "Lerp Node";
            node.rect = new Rect(pos.x, pos.y, 200, 50); ;

            NodeOutput.Create(node, "Value", "Float");

            return node;
        }

        protected override void NodeGUI()
        {
            value = RTEditorGUI.FloatField(new GUIContent("Value", "The input value of type float"), value);
            OutputKnob(0);

            if (GUI.changed)
                NodeEditor.RecalculateFrom(this);
        }

        public override bool Calculate()
        {
            Outputs[0].SetValue<float>(value);
            return true;
        }
    }
}
