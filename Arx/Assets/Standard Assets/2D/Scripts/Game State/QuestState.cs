using Assets.Standard_Assets.QuestSystem.QuestStructures;
using Assets.Standard_Assets.QuestSystem.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets._2D.Scripts.Game_State
{
    [Serializable]
    public class QuestState
    {
        public string QuestId { get; set; }
        public QuestStatus QuestStatus { get; set; }
        public ITask[] Tasks { get; set; }
    }
}
