using Assets.Standard_Assets.QuestSystem.Conditions;
using Assets.Standard_Assets.Utility.Editor;
using CommonEditors.GuiComponents.GuiComponents.CustomEditors;
using CommonEditors.GuiComponents.GuiComponents.GuiComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem.Editor.GuiComponent
{
    public class ConditionGuiComponent : BaseGuiComponent
    {
        private const string FoldoutTextTemplate = "Condition - {0}";
        private bool _showCondition = true;

        public ICondition Condition { get; private set; }

        public ConditionGuiComponent(ICondition condition)
        {
            Condition = condition;
        }

        public override void OnGui()
        {
            var currentIdentLevel = EditorGUI.indentLevel;
            _showCondition = EditorGUILayout.Foldout(_showCondition, Condition.GetType().Name);
            if (_showCondition)
            {
                EditorGUI.indentLevel++;
                var fields = ExposeFields.GetFields(Condition);
                ExposeFields.Expose(fields);
            }
            EditorGUI.indentLevel = currentIdentLevel;
            /*var editor = UnityEditor.Editor.CreateEditor(Condition, typeof(EditorWithoutScript));
            showQuest = EditorGUILayout.InspectorTitlebar(showQuest, editor.target);
            if (!showQuest)
            {
                return;
            }
            editor.OnInspectorGUI();*/
        }
    }
}
