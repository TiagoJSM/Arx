using Assets.Standard_Assets.QuestSystem.QuestStructures;
using CommonEditors.GuiComponents.GuiComponents.CustomEditors;
using CommonEditors.GuiComponents.GuiComponents.GuiComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem.Editor.Components
{
    public class QuestGuiComponent : BaseGuiComponent
    {
        private const string FoldoutText = "Quest";
        private bool showQuest = true;

        public Quest Quest { get; private set; }

        public QuestGuiComponent(Quest quest)
        {
            Quest = quest;
            quest.description = quest.description ?? string.Empty;
            quest.name = quest.name ?? string.Empty;
        }

        public override void OnGui()
        {
            var currentIdent = EditorGUI.indentLevel;
            showQuest = EditorGUILayout.Foldout(showQuest, FoldoutText);
            if (showQuest)
            {
                EditorGUI.indentLevel++;
                var editor = UnityEditor.Editor.CreateEditor(Quest, typeof(EditorWithoutScript));
                editor.OnInspectorGUI();
            }
            EditorGUI.indentLevel = currentIdent;
        }
    }
}
