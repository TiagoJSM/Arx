using AnimatorSequencer.MovementStates;
using AnimatorSequencerEditors.Nodes.Nodes.ActionSequence;
using CommonEditors.Nodes.Framework;
using CommonEditors.Nodes.Framework.CanvasSaveObjects;
using CommonEditors.Nodes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            node.rect = new Rect(pos.x, pos.y, 200, 50);

            NodeInput.Create(node, "", "Float");
            NodeOutput.Create(node, "", "Float");

            return node;
        }

        protected override void NodeGUI()
        {
            /*GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            float val = 0;

            if (Inputs[0].connection != null)
                GUILayout.Label(Inputs[0].name);
            else
                val = RTEditorGUI.FloatField(GUIContent.none, val);*/
            GUILayout.BeginHorizontal();
            Inputs[0].DisplayLayout();
            this.BaseActionSequence.name = GUILayout.TextField(this.BaseActionSequence.name);
            Outputs[0].DisplayLayout();
            GUILayout.EndHorizontal();
            /*GUILayout.EndVertical();
            GUILayout.BeginVertical();*/
            
            
            //value = RTEditorGUI.FloatField(new GUIContent("Value", "The input value of type float"), value);
            /*GUILayout.BeginHorizontal();
            GUILayout.Label("Name", GUILayout.ExpandWidth(true));
            this.BaseActionSequence.name = GUILayout.TextField(this.BaseActionSequence.name);

            GUILayout.EndHorizontal();*/

            /*GUILayout.EndVertical();
            GUILayout.EndHorizontal();*/

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
