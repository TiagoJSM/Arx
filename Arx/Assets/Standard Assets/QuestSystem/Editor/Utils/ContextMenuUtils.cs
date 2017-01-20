using Assets.Standard_Assets.QuestSystem.Conditions;
using Assets.Standard_Assets.QuestSystem.RewardProviders;
using Assets.Standard_Assets.QuestSystem.Tasks;
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
        private const string AddTaskTemplate = "Add task/{0}";
        private const string AddRewardProviderTemplate = "Add reward provider/{0}";

        public static GenericMenu GetcontextMenuForQuestEditor(GenericMenu.MenuFunction2 menuFunc)
        {
            var menu = new GenericMenu();
            var tasks = IntrospectionUtils.GetAllCompatibleTypes<ITask>();
            AddSubMenuItems(menu, tasks, AddTaskTemplate, menuFunc);
            var conditions = IntrospectionUtils.GetAllCompatibleTypes<ICondition>();
            AddSubMenuItems(menu, conditions, AddConditionTemplate, menuFunc);
            var rewardProvider = IntrospectionUtils.GetAllCompatibleTypes<IRewardProvider>();
            AddSubMenuItems(menu, rewardProvider, AddRewardProviderTemplate, menuFunc);
            return menu;
        }

        private static void AddSubMenuItems(
            GenericMenu menu, IEnumerable<Type> types, string templateText, GenericMenu.MenuFunction2 menuFunc)
        {
            foreach (var type in types)
            {
                menu.AddItem(
                    new GUIContent(string.Format(templateText, type.Name)),
                    false,
                    menuFunc,
                    type);
            }
        }
    }
}
