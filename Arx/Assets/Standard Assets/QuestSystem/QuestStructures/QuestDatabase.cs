using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.QuestSystem.QuestStructures
{
    [CreateAssetMenu(fileName = "QuestDB", menuName = "Quest Database", order = 1)]
    public class QuestDatabase : ScriptableObject
    {
        [SerializeField]
        private Quest[] _quests;

        public Quest[] Quests { get { return _quests; } }

        public bool HasQuest(string id)
        {
            return _quests.Any(quest => quest.questId == id);
        }

        public Quest GetQuest(string id)
        {
            return _quests.FirstOrDefault(quest => quest.questId == id);
        }

        public QuestDatabase Clone()
        {
            var questDb = CreateInstance<QuestDatabase>();
            questDb._quests = _quests.Select(quest => Instantiate(quest)).ToArray();
            return questDb;
        }
    }
}
