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

        public float value = 1f;

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
            GUILayout.Label("Time");
            this.ActionSequence.time = EditorGUILayout.FloatField(this.ActionSequence.time);
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
