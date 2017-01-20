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

        public bool Active { get; set; }

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
                if (!Active)
                {
                    return QuestStatus.Inactive;
                }
                if (tasks.All(c => c.Complete))
                {
                    return QuestStatus.Complete;
                }
                return QuestStatus.Active;
            }
        }

        public Quest()
        {
            conditions = new List<ICondition>();
            tasks = new List<ITask>();
            rewardProviders = new List<IRewardProvider>();
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

        public void GiveReward()
        {

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
    }
}

