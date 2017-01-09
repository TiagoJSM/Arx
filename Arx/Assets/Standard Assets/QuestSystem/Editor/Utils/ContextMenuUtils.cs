using Assets.Standard_Assets.QuestSystem.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Assets.Standard_Assets.QuestSystem.Editor.Utils
{
    public static class ContextMenuUtils
    {
        private const string AddConditionTemplate = "Add condition/{0}";

        public static GenericMenu GetcontextMenuForQuestEditor(GenericMenu.MenuFunction2 menuFunc)
        {
            GenericMenu menu = new GenericMenu();
            var conditions = IntrospectionUtils.GetAllCompatibleTypes<ICondition>();
            foreach (var condition in conditions)
            {
                menu.AddItem(
                    new GUIContent(string.Format(AddConditionTemplate, condition.Name)),
                    false, 
                    menuFunc,
                    condition);  
            }
            return menu;
        }
    }
}
