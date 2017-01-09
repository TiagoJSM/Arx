using CommonEditors.GuiComponents.GuiComponents.CustomEditors;
using CommonEditors.GuiComponents.GuiComponents.GuiComponents;
using QuestSystem.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace QuestSystemEditors.GuiComponent
{
    public class ConditionGuiComponent : BaseGuiComponent
    {
        private const string FoldoutTextTemplate = "Condition - {0}";
        private bool showQuest = true;

        public ICondition Condition { get; private set; }

        public ConditionGuiComponent(ICondition condition)
        {
            Condition = condition;
        }

        public override void OnGui()
        {
            var editor = Editor.CreateEditor(Condition, typeof(EditorWithoutScript));
            showQuest = EditorGUILayout.InspectorTitlebar(showQuest, editor.target);
            if (!showQuest)
            {
                return;
            }
            editor.OnInspectorGUI();
        }
    }
}
