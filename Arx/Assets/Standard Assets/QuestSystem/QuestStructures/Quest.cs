using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using CommonInterfaces.Inventory;
using GenericComponents.Behaviours;
using Assets.Standard_Assets.QuestSystem.Conditions;
using Assets.Standard_Assets.QuestSystem.Tasks;
using Assets.Standard_Assets.QuestSystem.RewardProviders;

namespace Assets.Standard_Assets.QuestSystem.QuestStructures
{
    [Serializable]
	public class Quest : SerializedScriptableObject, IEquatable<Quest>
	{
        [SerializeField]
        private bool _autocomplete;
        [SerializeField]
        [HideInInspector]
        private QuestStatus _questStatus;

        [HideInInspector]
        public string questId;
		public string questName;
        [TextArea(3, 3)]
		public string description;
        public List<ICondition> conditions;
        public List<ITask> tasks;
        public List<IRewardProvider> rewardProviders;

        public QuestStatus QuestStatus 
        {
            get
            {
                return _questStatus;
            }
            private set
            {
                _questStatus = value;
            }
        }

        public bool AllTasksComplete
        {
            get
            {
                return tasks.All(c => c.Complete);
            }
        }

        public Quest()
        {
            conditions = new List<ICondition>();
            tasks = new List<ITask>();
            rewardProviders = new List<IRewardProvider>();
        }

        public ITask GetTask(string name = null)
        {
            return
                GetTasks<ITask>()
                    .Where(task => name == null || task.TaskName == name).FirstOrDefault();
        }

        public TTask GetTask<TTask>(string name = null) where TTask : ITask
        {
            return
                GetTasks<TTask>()
                    .Where(task => name == null || task.TaskName == name).FirstOrDefault();
        }

        public IEnumerable<TTask> GetTasks<TTask>() where TTask : ITask
        {
            return
                tasks
                    .OfType<TTask>();
        }

        public void Activate()
        {
            QuestStatus = QuestStatus.Active;
        }

        public void Complete()
        {
            QuestStatus = QuestStatus.Complete;
            GiveReward();
        }

        private void Awake()
        {
            for(var idx = 0; idx < tasks.Count; idx++)
            {
                tasks[idx].OnTaskComplete += OnTaskCompleteHandler;
            }
        }

        private void GiveReward()
        {
            for(var idx = 0; idx < rewardProviders.Count; idx++)
            {
                rewardProviders[idx].GiveReward();
            }
        }

        private IEnumerable<ICondition> GetIncompleteConditions()
		{
			return conditions.Where(c => !c.Complete);
		}

        public override bool Equals(object obj)
        {
            var quest = obj as Quest;
            if (quest == null)
            {
                return base.Equals(obj);
            }
            return Equals(quest);
        }

        public bool Equals(Quest other)
        {
            return this.questId.Equals(other.questId);
        }

        private void OnTaskCompleteHandler(ITask task)
        {
            if (_autocomplete && AllTasksComplete)
            {
                Complete();
            }
        }
    }
}

