using AnimatorSequencer.States.ControlStates;
using CommonEditors.Nodes.Framework;
using CommonEditors.Nodes.Framework.CanvasSaveObjects;
using CommonEditors.Nodes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AnimatorSequencerEditors.Nodes.Nodes.ActionSequence.Control
{
    [Serializable]
    [Node(false, "Control/Wait")]
    public class WaitNode : BaseActionSequenceNode<WaitState>
    {
        public const string ID = "waitNode";
        public override string GetID { get { return ID; } }

        public float value = 1f;

        public WaitNode() : base(CreateInstance<WaitState>())
        {
        }

        public override Node Create(Vector2 pos)
        { // This function has to be registered in Node_Editor.ContextCallback
            var node = CreateInstance<WaitNode>();

            node.name = "Wait Node";
            node.BaseActionSequence.name = node.name;
            node.rect = new Rect(pos.x, pos.y, 200, 80); ;

            NodeInput.Create(node, "", "Float");
            NodeOutput.Create(node, "", "Float");

            return node;
        }

        protected override void NodeGUI()
        {
            GUILayout.BeginHorizontal();
            Inputs[0].DisplayLayout();

            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("State name");
            this.BaseActionSequence.name = GUILayout.TextField(this.BaseActionSequence.name);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Wait Time");
            this.ActionSequence.waitTimeInSeconds = EditorGUILayout.FloatField(this.ActionSequence.waitTimeInSeconds);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            Outputs[0].DisplayLayout();
            GUILayout.EndHorizontal();

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
