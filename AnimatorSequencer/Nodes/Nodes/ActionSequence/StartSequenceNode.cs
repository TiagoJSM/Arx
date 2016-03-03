using CommonEditors.Nodes.Framework;
using CommonEditors.Nodes.Framework.CanvasSaveObjects;
using CommonEditors.Nodes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer.Nodes.Nodes.ActionSequence
{
    [Serializable]
    [Node(false, "Start sequence")]
    public class StartSequenceNode : Node
    {
        public const string ID = "startSequenceNode";
        public override string GetID { get { return ID; } }

        public float value = 1f;

        public StartSequenceNode()
        {
            var texture = new Texture2D(1, 1);
            texture.SetPixels(new Color[] { new Color(0f, 0.5f, 0f) });
            texture.Apply();
            BackgroundColorTexture = texture;
        }

        public override Node Create(Vector2 pos)
        { // This function has to be registered in Node_Editor.ContextCallback
            var node = CreateInstance<StartSequenceNode>();

            var areaStyle = new GUIStyle();
            node.canBeDeleted = false;

            node.name = "Start Sequence Node";
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
