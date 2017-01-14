using Assets.Standard_Assets.QuestSystem.QuestStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem.EditorProperties
{
    [Serializable]
    public class TaskSelector
    {
        [SerializeField]
        private Quest _quest;
        [SerializeField]
        private string _taskName;

        public Quest Quest { get { return _quest; } }
        public string TaskName { get { return _taskName; } }
    }
}
