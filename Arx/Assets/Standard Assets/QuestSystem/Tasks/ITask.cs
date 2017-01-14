using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.QuestSystem.Tasks
{
    public interface ITask
    {
        bool Complete { get; }
        string TaskName { get; }
    }
}
