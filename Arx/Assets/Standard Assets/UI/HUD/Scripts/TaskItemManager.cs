using Assets.Standard_Assets.QuestSystem.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Standard_Assets.UI.HUD.Scripts
{
    public class TaskItemManager : MonoBehaviour
    {
        private ITask _task;

        [SerializeField]
        private Text _taskDescription;

        public ITask Task
        {
            get
            {
                return _task;
            }
            set
            {
                _task = value;
                _taskDescription.text = _task.TaskDescription;
            }
        }
    }
}
