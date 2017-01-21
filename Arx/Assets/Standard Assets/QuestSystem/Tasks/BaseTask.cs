using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.QuestSystem.Tasks
{
    [Serializable]
    public abstract class BaseTask : ITask
    {
        public string taskName;
        public string taskDescription;

        public abstract event OnTaskComplete OnTaskComplete;

        public string TaskName { get { return taskName; } }
        public abstract bool Complete { get; }
        public abstract string TaskDescription { get; }
    }
}
