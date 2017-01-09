using CommonEditors.GuiComponents.GuiComponents.CustomEditors;
using CommonEditors.GuiComponents.GuiComponents.GuiComponents;
using QuestSystem.QuestStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace QuestSystemEditors.Components
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
            showQuest = EditorGUILayout.Foldout(showQuest, FoldoutText);
            if (!showQuest)
            {
                return;
            }
            GUILayout.Label("Quest title");
            Quest.name = GUILayout.TextField(Quest.name);
            GUILayout.Label("Quest description");
            Quest.description = GUILayout.TextArea(Quest.description, GUILayout.Height(30));

            var editor = Editor.CreateEditor(Quest, typeof(EditorWithoutScript));
            editor.OnInspectorGUI();

        }
    }
}
