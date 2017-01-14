using Assets.Standard_Assets.QuestSystem.Attributes;
using Assets.Standard_Assets.QuestSystem.QuestStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem.Editor.AttributeDrawers
{
    [CustomPropertyDrawer(typeof(TaskSelectorAttribute))]
    public class TaskSelectorAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var questProperty = property.FindPropertyRelative("_quest");
            var taskNameProperty = property.FindPropertyRelative("_taskName");

            var editorPosition = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(editorPosition, questProperty);

            var quest = questProperty.objectReferenceValue as Quest;
            GUI.enabled = quest != null;

            editorPosition.y += EditorGUIUtility.singleLineHeight;
            var taskNames = new string[0];
            if(quest != null)
            {
                taskNames =
                    quest
                        .tasks
                        .Select(task => task.TaskName)
                        .Where(name => !string.IsNullOrEmpty(name))
                        .ToArray();
            }

            var selectedTaskIdx = GetCurrentSelectedTaskIndex(taskNames, taskNameProperty);
            selectedTaskIdx = EditorGUI.Popup(editorPosition, "Task", selectedTaskIdx, taskNames);
            if(selectedTaskIdx != -1)
            {
                taskNameProperty.stringValue = taskNames[selectedTaskIdx];
            }
        }

        private int GetCurrentSelectedTaskIndex(string[] taskNames, SerializedProperty taskNameProp)
        {
            var currentValue = taskNameProp.stringValue;
            for(var idx = 0; idx < taskNames.Length; idx++)
            {
                if(taskNames[idx] == currentValue)
                {
                    return idx;
                }
            }
            return -1;
        }
    }
}
