using Assets.Standard_Assets.QuestSystem.Tasks;
using Assets.Standard_Assets.Utility.Editor;
using CommonEditors.GuiComponents.GuiComponents.GuiComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Assets.Standard_Assets.QuestSystem.Editor.GuiComponent
{
    public class TaskGuiComponent : BaseGuiComponent
    {
        private const string FoldoutTextTemplate = "Task - {0}";
        private bool _showTask = true;

        public ITask Task { get; private set; }

        public TaskGuiComponent(ITask task)
        {
            Task = task;
        }

        public override void OnGui()
        {
            var currentIdentLevel = EditorGUI.indentLevel;
            _showTask = EditorGUILayout.Foldout(_showTask, Task.GetType().Name);
            if (_showTask)
            {
                EditorGUI.indentLevel++;
                var fields = ExposeFields.GetFields(Task);
                ExposeFields.Expose(fields);
            }
            EditorGUI.indentLevel = currentIdentLevel;
        }
    }
}
