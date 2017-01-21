using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.QuestSystem.Tasks
{
    public delegate void OnTaskComplete(ITask task);

    public interface ITask
    {
        bool Complete { get; }
        string TaskName { get; }
        string TaskDescription { get; }

        event OnTaskComplete OnTaskComplete;
    }
}
