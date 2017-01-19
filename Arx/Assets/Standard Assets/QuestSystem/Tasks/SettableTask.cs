using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.QuestSystem.Tasks
{
    [Serializable]
    public class SettableTask : BaseTask
    {
        private bool _complete;

        public override bool Complete
        {
            get { return _complete; }
        }

        public override string TaskDescription
        {
            get
            {
                return taskDescription;
            }
        }

        public void SetComplete()
        {
            _complete = true;
        }
    }
}
