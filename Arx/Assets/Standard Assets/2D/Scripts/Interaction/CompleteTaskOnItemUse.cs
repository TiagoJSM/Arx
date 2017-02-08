using Assets.Standard_Assets.InventorySystem.Triggers;
using Assets.Standard_Assets.QuestSystem.Attributes;
using Assets.Standard_Assets.QuestSystem.EditorProperties;
using Assets.Standard_Assets.QuestSystem.QuestStructures;
using Assets.Standard_Assets.QuestSystem.Tasks;
using CommonInterfaces.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Interaction
{
    public class CompleteTaskOnItemUse : ItemUseTrigger
    {
        [TaskSelector]
        [SerializeField]
        private TaskSelector _taskSelector;

        protected override bool DoUse(IItemOwner owner, IInventoryItem item)
        {
            var questLog = FindObjectOfType<QuestLogComponent>();
            var quest = questLog.GetQuest(_taskSelector.Quest.questId);
            var task = quest.GetTask<SettableTask>(_taskSelector.TaskName);
            task.SetComplete();
            return true;
        }
    }
}
