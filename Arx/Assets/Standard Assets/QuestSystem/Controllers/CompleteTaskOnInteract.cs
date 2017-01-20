using Assets.Standard_Assets.QuestSystem.Attributes;
using Assets.Standard_Assets.QuestSystem.EditorProperties;
using Assets.Standard_Assets.QuestSystem.QuestStructures;
using Assets.Standard_Assets.QuestSystem.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem.Controllers
{
    [RequireComponent(typeof(InteractibleCharacterController))]
    public class CompleteTaskOnInteract : MonoBehaviour
    {
        private InteractibleCharacterController _interactible;

        [TaskSelector]
        [SerializeField]
        private TaskSelector _taskSelector;

        private void Awake()
        {
            _interactible = GetComponent<InteractibleCharacterController>();
            _interactible.OnInteract += CompleteTask;
        }

        private void CompleteTask(GameObject interactor)
        {
            var questLog = interactor.GetComponent<QuestLogComponent>();
            var quest = questLog.GetQuest(_taskSelector.Quest.questId);
            var task = quest.GetTask<SettableTask>(_taskSelector.TaskName);
            if (task.Complete)
            {
                return;
            }
            task.SetComplete();
        }
    }
}
