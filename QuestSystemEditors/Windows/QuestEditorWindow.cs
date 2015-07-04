using CommonEditors;
using QuestSystem.QuestStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace QuestSystemEditors.Windows
{
    public class QuestEditorWindow : BaseWindow
    {
        public string QuestName { get; set; }
        public string QuestDescription { get; set; }
        private MonoBehaviour _quest;

        public QuestEditorWindow()
        {
            ClientRect = new Rect(50, 50, 300, 500);
            Title = "Quest";
            QuestName = string.Empty;
            QuestDescription = string.Empty;
        }

        public override void WindowFunction(int id)
        {
            GUILayout.Label("Quest title");
            QuestName = GUILayout.TextField(QuestName);
            GUILayout.Space(10);
            GUILayout.Label("Quest description");
            QuestDescription = GUILayout.TextArea(QuestDescription);
            _quest = EditorGUILayout.ObjectField("Drag Element Here to Add", _quest, typeof(MonoBehaviour), false) as MonoBehaviour;
            GUI.DragWindow();
        }
    }
}
