using CommonEditors;
using QuestSystemEditors.Utils;
using QuestSystemEditors.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace QuestSystemEditors
{
    public class QuestEditor : ExtendedEditorWindow
    {
        private List<BaseWindow> _windows;

        [MenuItem("Window/Quest Editor")]
        static void Init()
        {
            EditorWindow.GetWindow(typeof(QuestEditor));
        }

        public QuestEditor()
        {
            _windows = new List<BaseWindow>() { new QuestEditorWindow() };
            OnMouseUp += OnMouseUpHandler;
        }

        protected override void DoOnGui()
        {
            DrawWindows(_windows);
        }

        private void OnMouseUpHandler(MouseButton button, Vector2 position)
        {
            var toolsMenu = ContextMenuUtils.GetcontextMenuForQuestEditor(AddConditonWindow);
            toolsMenu.ShowAsContext();
        }

        private void AddConditonWindow(object conditionType)
        {

        }
    }
}
